using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using System.Collections.Generic;

namespace shop_giay_server._Controllers
{
    public class CartController : GeneralController<Cart, CartDTO>
    {
        public CartController(IAsyncRepository<Cart> repo, ILogger<CartController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCart model)
        {
            // Validate
            var cartExisted = await _repository.ExistWhere((c) => c.CustomerId == model.CustomerId);
            if (cartExisted)
            {
                return BadRequest(Response<Cart>.BadRequest("Cannot create more than one cart."));
            }

            var item = new Cart
            {
                CustomerId = model.CustomerId
            };

            return await this.AddItem(item);
        }
    }

    public class CreateCart
    {
        public int CustomerId { get; set; }
    }

    //public class InsertCartItem
    //{
    //    public List<CartItemDTO> items;
    //}
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using shop_giay_server.data;
//using shop_giay_server.models;
//using shop_giay_server._Services;
//using shop_giay_server._Repository;

//namespace shop_giay_server._Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CartController : ControllerBase
//    {
//        // private readonly IService<Cart> _service;
//        //private readonly IAsyncRepository<Cart> _repository;

//        //public CartController(IAsyncRepository<Cart> repository)
//        //{
//        //    _repository = repository;
//        //}

//        //// GET: api/Cart
//        //[HttpGet]
//        //public async Task<ActionResult<IEnumerable<Cart>>> GetAll()
//        //{
//        //    var items = await _repository.GetAll();
//        //    return Ok(new {
//        //        data = items,
//        //        code = 200
//        //    });
//        //}

//        // GET: api/Cart/5
//        //[HttpGet("{id}")]
//        //public async Task<ActionResult<Cart>> GetCart(int id)
//        //{
//        //    var cart = await _context.Carts.FindAsync(id);

//        //    if (cart == null)
//        //    {
//        //        return NotFound();
//        //    }

//        //    return cart;
//        //}

//        //// PUT: api/Cart/5
//        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
//        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
//        //[HttpPut("{id}")]
//        //public async Task<IActionResult> PutCart(int id, Cart cart)
//        //{
//        //    if (id != cart.Id)
//        //    {
//        //        return BadRequest();
//        //    }

//        //    _context.Entry(cart).State = EntityState.Modified;

//        //    try
//        //    {
//        //        await _context.SaveChangesAsync();
//        //    }
//        //    catch (DbUpdateConcurrencyException)
//        //    {
//        //        if (!CartExists(id))
//        //        {
//        //            return NotFound();
//        //        }
//        //        else
//        //        {
//        //            throw;
//        //        }
//        //    }

//        //    return NoContent();
//        //}

//        //// POST: api/Cart
//        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
//        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
//        //[HttpPost]
//        //public async Task<ActionResult<Cart>> PostCart(Cart cart)
//        //{
//        //    _context.Carts.Add(cart);
//        //    await _context.SaveChangesAsync();

//        //    return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
//        //}

//        //// DELETE: api/Cart/5
//        //[HttpDelete("{id}")]
//        //public async Task<ActionResult<Cart>> DeleteCart(int id)
//        //{
//        //    var cart = await _context.Carts.FindAsync(id);
//        //    if (cart == null)
//        //    {
//        //        return NotFound();
//        //    }

//        //    _context.Carts.Remove(cart);
//        //    await _context.SaveChangesAsync();

//        //    return cart;
//        //}

//        //private bool CartExists(int id)
//        //{
//        //    return _context.Carts.Any(e => e.Id == id);
//        //}
//    }
//}
