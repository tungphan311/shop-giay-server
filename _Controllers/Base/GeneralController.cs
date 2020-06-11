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


        #region HELPER METHOD FOR HANDLE GET REQUEST

        /*
            Summary:
            - Modify query from the request.
            - If derived class override the method, it should call base method.
        */
        protected virtual Task<IQueryCollection> TransformQueryFromGetAll(IQueryCollection query)
        {
            // Detect if need include items with delete flag
            var includeDeletedKey = "includeDeleted";
            if (query.ContainsKey(includeDeletedKey)) {
                if (query[includeDeletedKey].Count != 1) {
                    throw new InvalidQueryParamsException();
                }
                if (query[includeDeletedKey][0] != "true") {
                    var queryItem = new KeyValuePair<string, StringValues>("flagDelete", "false");
                    query.Append(queryItem);
                }
            }
            return Task.FromResult(query);
        }


        /*
            Summary:
            - Dynamic map from entites to specified response type 
            using AutoMapper + Genetic parameter.
        */
        private IEnumerable<ResponseModel> MapToResponseModels<ResponseModel>(IEnumerable<Model> entites)
            where ResponseModel : BaseDTO
        {
            var source = new Source<IEnumerable<Model>> { Value = entites };
            var result = _mapper.Map<Source<IEnumerable<Model>>, Destination<IEnumerable<ResponseModel>>>(source);
            return result.Value;
        }


        /*
            Summary:
            - Give a change to modify response models before return.
        */
        protected virtual async Task<IEnumerable<object>> FinishMapResponseModel(
            IEnumerable<object> responseEntities,
            IEnumerable<Model> entities)
        {
            return await Task.FromResult(responseEntities);
        }


        /*
            Summary:
            - Give a change to modify response models before return.
        */
        protected virtual async Task<object> FinishMapResponseModel(object responseEntity, Model entity)
        {
            return await Task.FromResult(responseEntity);
        }


        /*
            Summary: Return resposne for get request.
        */
        public async Task<IActionResult> ResultForGetAll(IEnumerable<dynamic> items)
        {
            if (items.Count() == 0)
            {
                return NotFound(Response<Model>.NotFound());
            }
            var totalRecords = await _repository.CountWhere(c => c.DeleteFlag == false);
            return Ok(Response<dynamic>.Ok(items, totalRecords));
        }


        /*
            Summary: Return error response for exception.
        */
        public IActionResult HandleExceptionInRequest(Exception e)
        {
            if (e is InvalidQueryParamsException)
            {
                return BadRequest(Response<Model>.BadRequest("Invalid query params."));
            } 
            else if (e is Microsoft.Data.SqlClient.SqlException)
            {
                return BadRequest(Response<Model>.BadRequest("Cannot connect to database."));
            }
            else
            {
                return BadRequest(Response<Model>.BadRequest());
            }
        }


        /*
            Summary: - Support get models with dynamic ResponseModel type.
        */
        protected async Task<IActionResult> _GetAllModels<ResponseModel>()
            where ResponseModel : BaseDTO
        {
            try
            {
                var query = this.Request.Query;

                // 1: Transform query
                query = await TransformQueryFromGetAll(query);

                // 2: Get entities from query
                var entities = await _repository.GetWithQuery(query);

                // 3: Map to ResponseModel
                dynamic responseItems = (object)MapToResponseModels<ResponseModel>(entities);

                // 4: Further modify after finished mapping
                responseItems = await FinishMapResponseModel(responseItems, entities);

                return await ResultForGetAll(responseItems);
            }
            catch (Exception e)
            {
                return HandleExceptionInRequest(e);
            }
        }

        #endregion


        #region ADMIN API

        /*
            Usage:
            - Overrided this method to customize ResponseModel Type
            by invoke _GetAllModels<T> with T is the specified type.
            - Because the ResponseModel type is mapped using AutoMapper,
            you need define MappingProfile in Startup.cs.
        */
        [Route("admin/[controller]")]
        [HttpGet]
        public virtual async Task<IActionResult> GetAllForAdmin()
        {
            return await _GetAllModels<DTO>();
        }


        [Route("admin/[controller]/{id:int}")]
        [HttpGet]
        public async virtual Task<IActionResult> GetByIdForAdmin(int id) 
        {
           return await _GetById<DTO>(id);
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


        #region CLIENT API

        /*
            Usage:
            - Overrided this method to customize ResponseModel Type
            by invoke _GetAllModels<T> with T is the specified type.
            - Because the ResponseModel type is mapped using AutoMapper,
            you need define MappingProfile in Startup.cs.
        */
        [Route("client/[controller]")]
        [HttpGet]
        public async virtual Task<IActionResult> GetAllForClient()
        {
            return await _GetAllModels<DTO>();
        }


        /*
            Usage:
            - Overrided this method to customize ResponseModel Type
            by invoke _GetById<T> with T is the specified type.
            - Because the ResponseModel type is mapped using AutoMapper,
            you need define MappingProfile for the specified type in Startup.cs.
        */
        [Route("client/[controller]/{id:int}")]
        [HttpGet]
        public async virtual Task<IActionResult> GetByIdForClient(int id) 
        {
           return await _GetById<DTO>(id);
        }

        #endregion


        #region HELPER METHODS

        protected async Task<IActionResult> _GetById<ResponseModel>(int id) 
            where ResponseModel: BaseDTO
        {
            try 
            {
                var entity = await _repository.GetById(id);
                if (entity == null) 
                {
                    return NotFound(Response<Model>.NotFound());
                }

                dynamic model = MapToResponseModels<ResponseModel>(new List<Model>() { entity }).First();
                model = await FinishMapResponseModel(model, entity);
                return Ok(Response<ResponseModel>.Ok(model));
            }
            catch (Exception e)
            {
                return HandleExceptionInRequest(e);
            }
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


class InvalidQueryParamsException: Exception {

}