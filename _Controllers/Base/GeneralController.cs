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

        #region Admin

        // GET: api/admin/[resources]
        [Route("admin/[controller]")]
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var dict = this.Request.Query.ToDictionary(
                p => p.Key.ToLower(),
                p => p.Value);
            return await _GetAll<DTO>(dict);
        }

        // DELETE: api/admin/[resources]/5
        [Route("admin/[controller]/{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteForAdmin(int id, bool setFlag = true)
        {
            return await DeleteItem(id, setFlag);
        }

        [Route("admin/[controller]")]
        [HttpDelete]
        public async Task<IActionResult> MultiDeleteForAdmin([FromQuery] List<int> ids, bool setFlag = true)
        {
            IActionResult response = BadRequest(Response<Model>.BadRequest());
            foreach (var id in ids)
            {
                response = await DeleteItem(id, setFlag);
            }

            return response;
        }


        // Helper method
        protected async Task<IActionResult> _UpdateItemForAdminAsync(Model model)
        {
            IActionResult response = BadRequest(Response<Model>.NotFound());
            if (!(await _repository.ExistWhere(m => m.Id == model.Id)))
            {
                return response;
            }

            var item = await _repository.Update(model);
            if (item == null)
            {
                return response;
            }

            response = Ok(Response<Model>.Ok(item));
            return response;
        }

        #endregion


        #region Client

        // GET: api/client/[resources]
        [Route("client/[controller]")]
        [HttpGet]
        public virtual Task<IActionResult> GetAllForClient()
        {
            IActionResult actionResult = BadRequest(Response<object>.BadRequest("This route has not been support"));
            return Task.FromResult(actionResult);
        }

        public async Task<IActionResult> _GetAll<ResponseDTO>(Dictionary<string, StringValues> queries) where ResponseDTO : BaseDTO
        {
            IActionResult actionResult = NotFound(Response<Model>.NotFound());

            // Detect if need include items with delete flag
            StringValues includeDeleted = "";
            if (!(queries.TryGetValue("includedeleted", out includeDeleted) && includeDeleted[0] == "true"))
            {
                queries["DeleteFlag"] = "false";
            }

            var items = await GetModels<ResponseDTO>(queries);
            var totalRecords = await _repository.CountWhere(c => c.DeleteFlag == false);

            if (items.Count > 0)
            {
                actionResult = Ok(Response<ResponseDTO>.Ok(items, totalRecords));
            }

            return actionResult;
        }

        private async Task<List<ResponseDTO>> GetModels<ResponseDTO>(Dictionary<string, StringValues> queries)
        {
            var items = await _repository.GetAll(queries);
            var source = new Source<List<Model>> { Value = items.ToList() };
            var result = _mapper.Map<Source<List<Model>>, Destination<List<ResponseDTO>>>(source);
            return result.Value;
        }


        // GET: api/client/[resources]/5
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

        #endregion


        #region Helper methods

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


        protected async Task<IActionResult> _AddItem(Model item)
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

        protected async Task<IActionResult> DeleteItem(int id, bool setFlag)
        {
            var item = await _repository.GetById(id);
            if (item == null)
            {
                return NotFound(Response<Model>.NotFound());
            }

            IActionResult response = BadRequest(Response<Model>.BadRequest());
            if (setFlag)
            {
                item.DeleteFlag = true;
                item = await _repository.Update(item);
                response = Ok(Response<Model>.OkDeleted(item));
            }
            else
            {
                var result = await _repository.Remove(id);
                response = result
                    ? Ok(Response<Model>.OkDeleted(item, "Removed from database."))
                    : response;
            }
            return response;
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
