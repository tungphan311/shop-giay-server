using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using shop_giay_server.data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace shop_giay_server._Controllers
{
    public class CartController : GeneralController<Cart, CartDTO>
    {
        public CartController(IAsyncRepository<Cart> repo, ILogger<CartController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCart model)
        {
            // Validate
            var cartExisted = await _repository.ExistWhere((c) => c.CustomerId == model.CustomerId);
            if (cartExisted)
            {
                return Ok(ResponseDTO.BadRequest("Cannot create more than one cart."));
            }

            var item = new Cart
            {
                CustomerId = model.CustomerId
            };

            return await this._AddItem(item);
        }


        #region CLIENT API


        //
        //  GET CART
        //
        [HttpGet]
        [Route("client/[controller]/get")]
        public async Task<IActionResult> ClientCartGet()
        {
            var customer = await GetCustomerFromSession();
            if (customer == null)
            {
                return Ok(ResponseDTO.BadRequest());
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(o => o.CustomerId == customer.Id);
            if (cart == null)
            {
                return Ok(ResponseDTO.OkEmpty());
            }

            var cartItems = await _context.CartItems
                .Include(c => c.Stock).ThenInclude(o => o.Shoes).ThenInclude(o => o.ShoesImages)
                .Include(c => c.Stock).ThenInclude(o => o.Size)
                .Where(o => o.CartId == cart.Id)
                .ToListAsync();

            var items = new List<dynamic>();
            foreach (var item in cartItems)
            {
                items.Add(new
                {
                    stockId = item.Stock.Id,
                    shoesId = item.Stock.ShoesId,
                    name = item.Stock.Shoes.Name,
                    sizeName = item.Stock.Size.Name,
                    quantity = item.Amount,
                    price = item.Stock.Shoes.Price,
                    image = item.Stock.Shoes.ShoesImages.FirstOrDefault().ImagePath
                });
            }

            return Ok(ResponseDTO.Ok(items, items.Count));
        }


        //
        //  UPDATE CART
        //
        [HttpPost]
        [Route("client/[controller]/update")]
        public async Task<IActionResult> ClientCartUpdate([FromBody] List<BodyCartDTO> updateList)
        {
            var customer = await GetCustomerFromSession();
            if (customer == null)
            {
                return Ok(ResponseDTO.BadRequest());
            }

            var cart = _context.Carts.FirstOrDefaultAsync(o => o.CustomerId == customer.Id);
            if (cart == null)
            {
                return Ok(ResponseDTO.OkEmpty());
            }

            var cartItems = _context.CartItems
                .Include(c => c.Stock).ThenInclude(o => o.Shoes).ThenInclude(o => o.ShoesImages)
                .Include(c => c.Stock).ThenInclude(o => o.Size)
                .Where(o => o.CartId == cart.Id);


            // Update details
            foreach (var detail in updateList)
            {
                var updateItem = cartItems.FirstOrDefault(c => c.Stock.Id == detail.stockId);
                if (updateItem != null)
                {
                    if (detail.quantity > 0)
                    {
                        updateItem.Amount = detail.quantity;
                    }
                    else
                    {
                        _context.CartItems.Remove(updateItem);
                    }
                }
            }
            _context.SaveChanges();


            // Parse responses
            var items = new List<dynamic>();
            foreach (var item in cartItems)
            {
                items.Add(new
                {
                    stockId = item.Stock.Id,
                    shoesId = item.Stock.ShoesId,
                    name = item.Stock.Shoes.Name,
                    sizeName = item.Stock.Size.Name,
                    quantity = item.Amount,
                    price = item.Stock.Shoes.Price,
                    image = item.Stock.Shoes.ShoesImages.FirstOrDefault().ImagePath
                });
            }

            return Ok(ResponseDTO.Ok(items, items.Count));
        }


        //
        //  ADD NEW CART ITEM
        //
        // Summary: If stockId is already existed, add quantity the existed one.
        [HttpPost]
        [Route("client/[controller]/add")]
        public async Task<IActionResult> ClientCartAddNewItem([FromBody] BodyCartDTO itemAdded)
        {
            var customer = await GetCustomerFromSession();
            if (customer == null || itemAdded.quantity <= 0)
            {
                return Ok(ResponseDTO.BadRequest());
            }

            Cart cart = await _context.Carts.FirstOrDefaultAsync(o => o.CustomerId == customer.Id);
            if (cart == null)
            {
                // Create cart
                var newCart = new Cart
                {
                    CustomerId = customer.Id
                };
                _context.Carts.Add(newCart);
                await _context.SaveChangesAsync();
                cart = newCart;
            }

            // get stock
            var stock = _context.Stocks
                .Include(s => s.Shoes).ThenInclude(s => s.ShoesImages)
                .Include(s => s.Size)
                .FirstOrDefault(s => s.Id == itemAdded.stockId);

            if (stock == null)
            {
                return Ok(ResponseDTO.BadRequest("StockId is invalid."));
            }

            var cartItems = _context.CartItems.Where(o => o.CartId == cart.Id);

            var existedItem = cartItems.FirstOrDefault(c => c.StockId == stock.Id);
            if (existedItem != null)
            {
                existedItem.Amount += itemAdded.quantity;
                _context.SaveChanges();
            }
            else
            {
                // new item cart
                var itemCart = new CartItem
                {
                    CartId = cart.Id,
                    Amount = itemAdded.quantity,
                    StockId = stock.Id
                };
                _context.CartItems.Add(itemCart);
                _context.SaveChanges();
                existedItem = itemCart;
            }

            var responseItem = new
            {
                stockId = stock.Id,
                shoesId = stock.ShoesId,
                name = stock.Shoes.Name,
                sizeName = stock.Size.Name,
                quantity = existedItem.Amount,
                price = stock.Shoes.Price,
                image = stock.Shoes.ShoesImages.FirstOrDefault().ImagePath
            };
            return Ok(ResponseDTO.Ok(responseItem));
        }


        //
        //  SYNC CART ITEMS 
        //
        [HttpPost]
        [Route("client/[controller]/sync")]
        public async Task<IActionResult> ClientCartSync([FromBody] List<BodyCartDTO> syncList)
        {
            var customer = await GetCustomerFromSession();
            if (customer == null)
            {
                return Ok(ResponseDTO.BadRequest());
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(o => o.CustomerId == customer.Id);
            if (cart == null)
            {
                // Create cart
                var newCart = new Cart
                {
                    CustomerId = customer.Id
                };
                _context.Carts.Add(newCart);
                await _context.SaveChangesAsync();
                cart = newCart;
            }

            var cartItems = _context.CartItems.Where(o => o.CartId == cart.Id);
            _context.CartItems.RemoveRange(cartItems);

            // Filter syncList
            syncList = syncList.Where(o => o.quantity > 0).ToList();

            // New list
            var newItems = new List<CartItem>();
            foreach (var detail in syncList)
            {
                if (await _context.Stocks.AnyAsync(s => s.Id == detail.stockId))
                {
                    var item = new CartItem
                    {
                        CartId = cart.Id,
                        Amount = detail.quantity,
                        StockId = detail.stockId
                    };
                    newItems.Add(item);
                }
            }
            _context.CartItems.AddRange(newItems);
            await _context.SaveChangesAsync();

            // Load references
            newItems = _context.CartItems
                .Include(c => c.Stock).ThenInclude(o => o.Shoes).ThenInclude(o => o.ShoesImages)
                .Include(c => c.Stock).ThenInclude(o => o.Size)
                .Where(c => c.CartId == cart.Id)
                .ToList();

            // Parse responses
            var items = new List<dynamic>();
            foreach (var item in cartItems)
            {
                items.Add(new
                {
                    stockId = item.Stock.Id,
                    shoesId = item.Stock.ShoesId,
                    name = item.Stock.Shoes.Name,
                    sizeName = item.Stock.Size.Name,
                    quantity = item.Amount,
                    price = item.Stock.Shoes.Price,
                    image = item.Stock.Shoes.ShoesImages.FirstOrDefault().ImagePath
                });
            }

            return Ok(ResponseDTO.Ok(items, items.Count));
        }

        #endregion
    }

    public class CreateCart
    {
        public bool DeleteFlag { get; set; } = false;
        public int CustomerId { get; set; }
    }

    public class BodyCartDTO
    {
        public int stockId { get; set; }
        public int quantity { get; set; }
    }

}
