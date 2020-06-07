using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_giay_server.models;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using System;
using Microsoft.Extensions.Primitives;

namespace shop_giay_server._Controllers
{
    [Route("api")]
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

        // GET: api/admin/[resources]
        [Route("admin/[controller]")]
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var dict = this.Request.Query.ToDictionary(
                p => p.Key.ToLower(),
                p => p.Value);
            return await _GetAllForClient<DTO>(dict);
        }

        // GET: api/client/[resources]
        [Route("client/[controller]")]
        [HttpGet]
        public virtual Task<IActionResult> GetAllForClient()
        {
            IActionResult actionResult = BadRequest(Response<object>.BadRequest("This route has not been support"));
            return Task.FromResult(actionResult);
        }

        private async Task<List<ResponseDTO>> GetModels<ResponseDTO>(Dictionary<string, StringValues> queries)
        {
            var items = await _repository.GetAll(queries);
            items = items.Where(i => i.DeleteFlag == false);
            var source = new Source<List<Model>> { Value = items.ToList() };
            var result = _mapper.Map<Source<List<Model>>, Destination<List<ResponseDTO>>>(source);
            return result.Value;
        }

        public async Task<IActionResult> _GetAllForClient<ResponseDTO>(Dictionary<string, StringValues> queries) where ResponseDTO : BaseDTO
        {
            IActionResult actionResult = NotFound(Response<Model>.NotFound());

            var items = await GetModels<ResponseDTO>(queries);
            if (items.Count > 0)
            {
                actionResult = Ok(Response<ResponseDTO>.Ok(items));
            }

            return actionResult;
        }

        // GET: api/import/5
        [Route("client/[controller]")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            var res = new Response<Model>(new List<Model>() { item });
            return Ok(res);
        }


        [Route("client/[controller]/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> GetForClient(int id)
        {
            return await _GetForClient<DTO>(id);
        }


        protected virtual async Task<IActionResult> _GetForClient<ResponseDTO>(int id) where ResponseDTO : BaseDTO
        {
            IActionResult actionResult = NotFound(Response<Model>.NotFound());
            var item = await _repository.GetById(id);
            var source = new Source<Model> { Value = item };
            var result = _mapper.Map<Source<Model>, Destination<ResponseDTO>>(source);

            if (item != null)
            {
                actionResult = Ok(Response<ResponseDTO>.Ok(result.Value));
            }

            return actionResult;
        }



        #region Helper methods

        public async Task<IActionResult> AddItem(Model item)
        {
            IActionResult result = BadRequest(Response<Model>.BadRequest());
            try
            {
                var insertedItem = await _repository.Add(item);
                result = Ok(Response<Model>.Ok(insertedItem));
            }
            catch (DbUpdateException ex)
            {
                var res = Response<Model>.BadRequest($"Cannot add new { typeof(Model).Name }.");
                result = BadRequest(res);
                _logger.LogError(ex.ToString());
            }

            return result;
        }


        #endregion

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
