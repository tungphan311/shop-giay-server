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
using shop_giay_server.Dtos;
using AutoMapper;

namespace shop_giay_server._Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController<Model, DTO> : ControllerBase
        where Model : BaseEntity
        where DTO : BaseDTO
    {
        protected readonly IAsyncRepository<Model> _repository;
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;


        public GeneralController(IAsyncRepository<Model> repository, ILogger logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/import
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var queries = this.Request.Query;
            var items = await _repository.GetAll(queries);


            var source = new Source<List<Model>> { Value = items.ToList() };
            var result = _mapper.Map<Source<List<Model>>, Destination<List<DTO>>>(source);

            // var res = new Response<List<DTO>>(result.Value);
            var res = new Response<DTO>(result.Value);
            return Ok(res);
        }


        // GET: api/import/5
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

        [HttpPost]
        public async Task<IActionResult> Add(DTO dto)
        {
            var source = new Source<DTO>() { Value = dto };
            var result = _mapper.Map<Source<DTO>, Destination<Model>>(source);
            return Ok(result);
        }
    }

    public class Source<T>
    {
        public T Value { get; set; }
    }

    public class Destination<T>
    {
        public T Value { get; set; }
    }
}
