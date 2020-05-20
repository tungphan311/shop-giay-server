using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_giay_server.data;
using shop_giay_server.models;
using shop_giay_server._Services;
using shop_giay_server._Repository;
using shop_giay_server._Controllers;
using Microsoft.Extensions.Logging;

namespace shop_giay_server._Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController<Model>: ControllerBase where Model : BaseEntity
    {
        private readonly IAsyncRepository<Model> _repository;
        private readonly ILogger _logger;


        public GeneralController(IAsyncRepository<Model> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }


        // GET: api/import
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var queries = this.Request.Query;
            var items = await _repository.GetAll(queries);
            var res = new Response<Model>(new List<Model>(items));
            return Ok(res);
        }


        //        // GET: api/import/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCart(int id)
        {
            var item = await _repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            var res = new Response<Model>(new List<Model>() { item });
            return Ok(res);
        }
    }

    public class GeneralQueryString
    {

    }
}
