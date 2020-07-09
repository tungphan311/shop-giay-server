using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Collections.Generic;
using shop_giay_server.data;
using Microsoft.EntityFrameworkCore;
using System;

namespace shop_giay_server._Controllers
{
    public class OrderController : GeneralController<Order, OrderDTO>
    {

        enum OrderStatus
        {
            Waiting = 1,
            Confirmed = 2,
            Cancelled = 3
        }

        enum PaymentStatus
        {
            NotYet = 1,
            Paid = 2
        }

        public OrderController(IAsyncRepository<Order> repo, ILogger<OrderController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        {
        }

        #region Admin's API

        protected override async Task<IEnumerable<object>> FinishMappingResponseModels(
                    IEnumerable<object> responseEntities,
                    IEnumerable<Order> entities,
                    RequestContext requestContext)
        {
            switch (requestContext.APIRoute)
            {
                case APIRoute.AdminGetAll:
                    foreach (OrderDTO order in responseEntities)
                    {
                        CustomizeOrderItem(order);
                    }
                    break;

                case APIRoute.AdminGetByID:
                    var dto = (OrderDTO)responseEntities.FirstOrDefault();
                    CustomizeOrderItem(dto);
                    break;

                default:
                    break;
            }

            return await base.FinishMappingResponseModels(responseEntities, entities, requestContext);
        }


        private void CustomizeOrderItem(OrderDTO dto)
        {
            foreach (OrderItemLiteDTO orderItem in dto.OrderItems)
            {
                var shoesInfo = GetShoesInfoByStockId(orderItem.StockId);
                orderItem.ShoesName = shoesInfo.name;
                orderItem.ImagePath = shoesInfo.imagePath;
                orderItem.ShoesId = shoesInfo.id;
            }
        }


        private (int id, string name, string imagePath) GetShoesInfoByStockId(int stockId)
        {
            var name = "";
            var imagePath = "";
            var id = 0;
            var stock = _context.Stocks
                .Include(c => c.Shoes)
                .ThenInclude(c => c.ShoesImages)
                .FirstOrDefault(o => o.Id == stockId);
            if (stock.Shoes != null)
            {
                if (!String.IsNullOrEmpty(stock.Shoes.Name)) { name = stock.Shoes.Name; id = stock.Shoes.Id; };

                var shoesImage = stock.Shoes.ShoesImages.FirstOrDefault();
                if (!String.IsNullOrEmpty(shoesImage.ImagePath)) imagePath = shoesImage.ImagePath;
            }
            return (id, name, imagePath);
        }

        // Update Order
        [Route("admin/[controller]/{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateOrderDTO dto)
        {
            if (id != dto.Id)
            {
                return Ok(ResponseDTO.BadRequest("URL ID and Item ID does not matched."));
            }

            // Validate
            var entity = await _repository.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return Ok(ResponseDTO.BadRequest("Item ID is not existed."));
            }

            entity.Status = dto.Status;
            entity.ConfirmDate = dto.ConfirmDate;
            entity.CancelDate = dto.CancelDate;
            entity.BeginDelivery = dto.BeginDelivery;
            entity.DeliveryDate = dto.DeliveryDate;
            entity.Note = dto.Note;

            await _repository.Update(entity);

            return Ok(ResponseDTO.Ok(entity));
        }

        #endregion

        #region Client's API

        [HttpPost()]
        [Route("client/[controller]")]
        public async Task<IActionResult> ClientProcessOrder([FromQuery(Name = "addressId")] int addressId)
        {
            // Get customer
            var customer = GetCustomer();
            if (customer == null)
            {
                return BadRequest(ResponseDTO.BadRequest("Invalid customer's username."));
            }

            // Check addressId
            var sendAddress = customer.Addresses.FirstOrDefault(o => o.Id == addressId);
            if (sendAddress == null
                || string.IsNullOrEmpty(sendAddress.RecipientName)
                || string.IsNullOrEmpty(sendAddress.RecipientPhoneNumber)
                )
            {
                return BadRequest(ResponseDTO.BadRequest($"Invalid address for current customer: {customer.Username}."));
            }

            // Check cart
            var cartItems = customer.Cart.CartItems;
            if (customer.Cart.CartItems.Count == 0)
            {
                return BadRequest(ResponseDTO.BadRequest($"No items in cart for current customer: {customer.Username}"));
            }

            // Convert cart items to order items
            // 1: Create order
            var newOrder = new Order()
            {
                OrderDate = DateTime.Now,
                Total = 0,
                Status = (int)OrderStatus.Waiting,
                DeliverAddress = sendAddress.ToString(),
                ConfirmDate = null,
                CancelDate = null,
                BeginDelivery = null,
                DeliveryDate = null,
                Note = "",
                RecipientName = sendAddress.RecipientName,
                RecipientPhoneNumber = sendAddress.RecipientPhoneNumber,
                CustomerId = customer.Id,
                OrderItems = new List<OrderItem>(),
                AddressId = addressId
            };
            newOrder = await _repository.Add(newOrder);

            // 2: Create order items
            var orderItems = new List<OrderItem>();
            float orderTotal = 0;
            foreach (var ci in cartItems)
            {
                var item = new OrderItem()
                {
                    Amount = ci.Amount,
                    PricePerUnit = ci.Stock.Shoes.Price,
                    OrderId = newOrder.Id,
                    StockId = ci.StockId,
                    Total = 0
                };

                var priceWithSale = item.PricePerUnit;
                var productSale = await _context.SaleProducts
                                    .Include(c => c.Sale)
                                    .FirstOrDefaultAsync(c => c.Shoes.Id == ci.Stock.Shoes.Id);
                if (productSale != null && productSale.Sale.Status != 0)
                {
                    var sale = productSale.Sale;
                    priceWithSale -= sale.SaleType == 1
                        ? priceWithSale * (float)((float)sale.Amount / 100.0)
                        : sale.Amount;
                }
                item.Total = item.Amount * priceWithSale;
                orderTotal += item.Total;

                // Update stock
                var stock = _context.Stocks.FirstOrDefault(c => c.Id == item.StockId);
                if (stock != null)
                {
                    // TODO: Check
                    stock.Instock -= item.Amount;
                }

                // Update cartItems in database
                _context.CartItems.Remove(ci);

                orderItems.Add(item);
            }
            newOrder.OrderItems = orderItems;
            newOrder.Total = orderTotal;
            await _context.SaveChangesAsync();

            // Response
            var responseDTO = new ClientOrderResponseDTO()
            {
                id = newOrder.Id,
                customerID = customer.Id,
                saleID = 0,
                city = sendAddress.City,
                orderDate = newOrder.OrderDate,
                confirmDate = newOrder.ConfirmDate,
                deliveryDate = newOrder.DeliveryDate,
                total = newOrder.Total,
                status = newOrder.Status,
                paymentStatus = newOrder.DeliveryDate.HasValue
                    ? (int)PaymentStatus.Paid
                    : (int)PaymentStatus.NotYet,
                deliveryAddress = newOrder.DeliverAddress,
                recipientName = sendAddress.RecipientName,
                recipientPhoneNumber = sendAddress.RecipientPhoneNumber,
                cartItemDTOList = orderItems.Select(c => new ClientOrder_CartItemDTO
                {
                    stockId = c.StockId,
                    shoesId = c.Stock.ShoesId,
                    name = c.Stock.Shoes.Name,
                    sizeName = c.Stock.Size.Name,
                    quantity = c.Amount,
                    price = c.Total,
                    image = c.Stock.Shoes.ShoesImages.FirstOrDefault().ImagePath
                }).ToList()
            };

            return Ok(ResponseDTO.Ok(responseDTO));
        }


        [Route("client/[controller]/{id:int}")]
        [HttpGet]
        public async override Task<IActionResult> GetByIdForClient(int id)
        {
            // Get customer
            var customer = GetCustomer();
            if (customer == null)
            {
                return BadRequest(ResponseDTO.BadRequest("Invalid customer's username."));
            }

            // Get order   
            var newOrder = customer.Orders.FirstOrDefault(c => c.Id == id);
            if (newOrder == null)
            {
                return BadRequest(ResponseDTO.BadRequest($"No order with id {id} for username: {customer.Username}."));
            }

            // Get address
            var sendAddress = await _context.Addresses.FirstOrDefaultAsync(c => c.Id == (newOrder.AddressId ?? 0));
            if (sendAddress == null)
            {
                return BadRequest(ResponseDTO.BadRequest("Invalid addrss for current order."));
            }

            // Response
            var orderItems = newOrder.OrderItems;
            List<ClientOrder_CartItemDTO> cartItemDTOList = new List<ClientOrder_CartItemDTO>() { };
            foreach (var item in orderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == item.StockId);
                var shoes = await _context.Shoes.FirstOrDefaultAsync(s => s.Id == stock.ShoesId);
                var image = await _context.ShoesImages.FirstOrDefaultAsync(i => i.ShoesId == item.Stock.ShoesId);
                var size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == stock.SizeId);
                var cart = new ClientOrder_CartItemDTO
                {
                    stockId = item.Id,
                    shoesId = shoes.Id,
                    name = shoes.Name,
                    sizeName = size.Name,
                    quantity = item.Amount,
                    price = item.Total,
                    image = image != null ? image.ImagePath : ""
                };
                cartItemDTOList.Add(cart);
            }

            var responseDTO = new ClientOrderResponseDTO()
            {
                id = newOrder.Id,
                customerID = customer.Id,
                saleID = 0,
                city = sendAddress.City,
                orderDate = newOrder.OrderDate,
                confirmDate = newOrder.ConfirmDate,
                deliveryDate = newOrder.DeliveryDate,
                total = newOrder.Total,
                status = newOrder.Status,
                paymentStatus = newOrder.DeliveryDate.HasValue
                                ? (int)PaymentStatus.Paid
                                : (int)PaymentStatus.NotYet,
                deliveryAddress = newOrder.DeliverAddress,
                recipientName = sendAddress.RecipientName,
                recipientPhoneNumber = sendAddress.RecipientPhoneNumber,
                cartItemDTOList = cartItemDTOList
                // cartItemDTOList = orderItems.Select(c => new ClientOrder_CartItemDTO
                // {
                //     stockId = c.StockId,
                //     shoesId = c.Stock.ShoesId,
                //     name = c.Stock.Shoes.Name,
                //     sizeName = c.Stock.Size.Name,
                //     quantity = c.Amount,
                //     price = c.Total,
                //     image = c.Stock.Shoes.ShoesImages.FirstOrDefault().ImagePath
                // }).ToList()
            };

            return Ok(ResponseDTO.Ok(responseDTO));
        }

        [Route("client/[controller]/list")]
        [HttpGet]
        public async Task<IActionResult> ClientGetOrderList()
        {
            // Get customer
            var customer = GetCustomer();
            if (customer == null)
            {
                return Ok(ResponseDTO.BadRequest("Invalid customer's username."));
            }

            // Get orders
            var orders = customer.Orders;
            var listResults = new List<ClientOrderResponseDTO>();
            foreach (var order in orders)
            {
                // Get address
                var sendAddress = await _context.Addresses.FirstOrDefaultAsync(c => c.Id == (order.AddressId ?? 0));
                if (sendAddress == null)
                {
                    // return Ok(ResponseDTO.BadRequest("Invalid addrss for current order."));
                    continue;
                }

                // Response
                var orderItems = order.OrderItems;
                List<ClientOrder_CartItemDTO> cartItemDTOList = new List<ClientOrder_CartItemDTO>() { };
                foreach (var item in orderItems)
                {
                    var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == item.StockId);
                    var shoes = await _context.Shoes.FirstOrDefaultAsync(s => s.Id == stock.ShoesId);
                    var image = await _context.ShoesImages.FirstOrDefaultAsync(i => i.ShoesId == item.Stock.ShoesId);
                    var size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == stock.SizeId);
                    var cart = new ClientOrder_CartItemDTO
                    {
                        stockId = item.Id,
                        shoesId = shoes.Id,
                        name = shoes.Name,
                        sizeName = size.Name,
                        quantity = item.Amount,
                        price = item.Total,
                        image = image != null ? image.ImagePath : ""
                    };
                    cartItemDTOList.Add(cart);
                }
                var responseDTO = new ClientOrderResponseDTO()
                {
                    id = order.Id,
                    customerID = customer.Id,
                    saleID = 0,
                    city = sendAddress.City,
                    orderDate = order.OrderDate,
                    confirmDate = order.ConfirmDate,
                    deliveryDate = order.DeliveryDate,
                    total = order.Total,
                    status = order.Status,
                    paymentStatus = order.DeliveryDate.HasValue
                                    ? (int)PaymentStatus.Paid
                                    : (int)PaymentStatus.NotYet,
                    deliveryAddress = order.DeliverAddress,
                    recipientName = sendAddress.RecipientName,
                    recipientPhoneNumber = sendAddress.RecipientPhoneNumber,
                    cartItemDTOList = cartItemDTOList
                    // cartItemDTOList = orderItems.Select(c => new ClientOrder_CartItemDTO
                    // {
                    //     stockId = c.StockId,
                    //     shoesId = c.Stock.ShoesId,
                    //     name = c.Stock.Shoes.Name,
                    //     sizeName = c.Stock.Size.Name,
                    //     quantity = c.Amount,
                    //     price = c.Total,
                    //     image = c.Stock.Shoes.ShoesImages.FirstOrDefault().ImagePath
                    // }).ToArray().ToList()
                };

                listResults.Add(responseDTO);
            }
            return Ok(ResponseDTO.Ok(listResults));
        }

        #endregion

        #region HELPER METHODS

        public Customer GetCustomer()
        {
            var sessionUsername = HttpContext.Session.GetString(SessionConstant.Username);
            if (string.IsNullOrEmpty(sessionUsername))
            {
                return null;
            }

            var customer = _context.Customers
                .Include(c => c.Addresses)
                .Include(c => c.Cart).ThenInclude(c => c.CartItems).ThenInclude(c => c.Stock).ThenInclude(c => c.Shoes).ThenInclude(c => c.ShoesImages)
                .Include(c => c.Cart).ThenInclude(c => c.CartItems).ThenInclude(c => c.Stock).ThenInclude(c => c.Size)
                .Include(c => c.Orders).ThenInclude(c => c.OrderItems)
                .FirstOrDefault(c => c.Username == sessionUsername);
            return customer;
        }

        #endregion

    }

    public class UpdateOrderDTO
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? BeginDelivery { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string Note { get; set; }
    }

    public class ClientOrderResponseDTO
    {
        public int id { get; set; }
        public int customerID { get; set; }
        public int saleID { get; set; }
        public string city { get; set; }
        public DateTime? orderDate { get; set; }
        public DateTime? confirmDate { get; set; }
        public DateTime? deliveryDate { get; set; }
        public float total { get; set; }
        public int status { get; set; }
        public int paymentStatus { get; set; }
        public string deliveryAddress { get; set; }
        public string recipientName { get; set; }
        public string recipientPhoneNumber { get; set; }
        public List<ClientOrder_CartItemDTO> cartItemDTOList { get; set; } = new List<ClientOrder_CartItemDTO>();
    }

    public class ClientOrder_CartItemDTO
    {
        public int stockId { get; set; }
        public int shoesId { get; set; }
        public string name { get; set; }
        public string sizeName { get; set; }
        public int quantity { get; set; }
        public float price { get; set; }
        public string image { get; set; }
    }
}
