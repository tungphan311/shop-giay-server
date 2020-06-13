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
using shop_giay_server.Helpers;

namespace shop_giay_server._Controllers
{
    public struct RequestContext
    {
        public RequestContext(APIRoute route, object contextValue = null)
        {
            this.APIRoute = route;
            this.ContextValue = contextValue;
        }

        public object ContextValue { get; set; }
        public readonly APIRoute APIRoute { get; }

        public static RequestContext AdminGetByID(int id)
        {
            return new RequestContext(APIRoute.AdminGetByID, id);
        }

        public static RequestContext AdminGetAll()
        {
            return new RequestContext(APIRoute.AdminGetAll);
        }

        public static RequestContext ClientGetByID(int id)
        {
            return new RequestContext(APIRoute.ClientGetByID, id);
        }

        public static RequestContext ClientGetAll()
        {
            return new RequestContext(APIRoute.ClientGetAll);
        }
    }

    public enum APIRoute
    {
        AdminGetAll,
        AdminGetByID,
        ClientGetAll,
        ClientGetByID
    }


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
            - If derived class override the method for further modifying, it should call base method.
        */
        protected virtual Task<IQueryCollection> TransformQuery(IQueryCollection query, RequestContext requestContext)
        {
            // Dictionary<string, StringValues> dict = query.ToDictionary(c => c.Key, c => c);
            switch (requestContext.APIRoute)
            {
                case APIRoute.AdminGetAll:
                case APIRoute.ClientGetAll:

                    // Detect if need include items with delete flag
                    var includeDeletedKey = "includeDeleted";
                    var defaultDeleteFlag = "false";
                    if (query.ContainsKey(includeDeletedKey))
                    {
                        if (query[includeDeletedKey].Count != 1)
                        {
                            throw new InvalidQueryParamsException();
                        }

                        if (query[includeDeletedKey][0] == "true")
                        {
                            defaultDeleteFlag = "true";
                        }
                    }
                    var queryItem = new KeyValuePair<string, StringValues>("DeleteFlag", defaultDeleteFlag);
                    query = query.AddQueryItem(queryItem);
                    break;

                default:
                    break;
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
        protected virtual async Task<IEnumerable<object>> FinishMappingResponseModels(
            IEnumerable<object> responseEntities,
            IEnumerable<Model> entities,
            RequestContext requestContext)
        {
            return await Task.FromResult(responseEntities);
        }


        /*
            Summary:
            - Give a change to modify response models before return.
        */
        // protected virtual async Task<object> FinishMapResponseModel(object responseEntity, Model entity)
        // {
        //     return await Task.FromResult(responseEntity);
        // }


        /*
            Summary: Return resposne for get request.
        */
        public async Task<IActionResult> ResultForGetAll(IEnumerable<dynamic> items)
        {
            if (items.Count() == 0)
            {
                return NotFound(ResponseDTO.NotFound());
            }
            var totalRecords = await _repository.CountWhere(c => c.DeleteFlag == false);
            return Ok(ResponseDTO.Ok(items, totalRecords));
        }


        /*
            Summary: Return error response for exception.
        */
        public IActionResult HandleExceptionInRequest(Exception e)
        {
            if (e is InvalidQueryParamsException)
            {
                return BadRequest(ResponseDTO.BadRequest("Invalid query params.", e.ToString()));
            }
            else if (e is Microsoft.Data.SqlClient.SqlException)
            {
                return BadRequest(ResponseDTO.BadRequest("Cannot connect to database.", e.ToString()));
            }
            else if (e is NullReferenceException)
            {
                return BadRequest(ResponseDTO.BadRequest("Items not existed.", e.ToString()));
            }
            else
            {
                return BadRequest(ResponseDTO.BadRequest());
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
            var context = RequestContext.AdminGetAll();
            return await _GetAllModels<DTO>(context);
        }


        [Route("admin/[controller]/{id:int}")]
        [HttpGet]
        public async virtual Task<IActionResult> GetByIdForAdmin(int id)
        {
            var context = RequestContext.AdminGetByID(id);
            return await _GetById<DTO>(id, context);
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
            IActionResult response = BadRequest(ResponseDTO.BadRequest());
            foreach (var id in ids)
            {
                response = await DeleteItem(id, setFlag);
            }

            return response;
        }


        // Helper method
        protected async Task<IActionResult> _UpdateItemForAdminAsync(Model model)
        {
            IActionResult response = BadRequest(ResponseDTO.NotFound());
            if (!(await _repository.ExistWhere(m => m.Id == model.Id)))
            {
                return response;
            }

            var item = await _repository.Update(model);
            if (item == null)
            {
                return response;
            }

            response = Ok(ResponseDTO.Ok(item));
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
            var context = RequestContext.ClientGetAll();
            return await _GetAllModels<DTO>(context);
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
            var context = RequestContext.ClientGetByID(id);
            return await _GetById<DTO>(id, context);
        }

        #endregion


        #region HELPER METHODS

        /*
            Summary: - Support get models with dynamic ResponseModel type.
        */
        protected async Task<IActionResult> _GetAllModels<ResponseModel>(RequestContext context)
            where ResponseModel : BaseDTO
        {
            try
            {
                var query = this.Request.Query;

                // 1: Transform query
                query = await TransformQuery(query, context);

                // 2: Get entities from query
                var entities = await _repository.GetWithQuery(query);

                // 3: Map to ResponseModel
                dynamic responseItems = (object)MapToResponseModels<ResponseModel>(entities);

                // 4: Further modify after finished mapping
                responseItems = await FinishMappingResponseModels(responseItems, entities, context);

                return await ResultForGetAll(responseItems);
            }
            catch (Exception e)
            {
                return HandleExceptionInRequest(e);
            }
        }

        protected async Task<IActionResult> _GetById<ResponseModel>(int id, RequestContext context)
            where ResponseModel : BaseDTO
        {
            try
            {
                var entity = await _repository.GetById(id);
                if (entity == null)
                {
                    return NotFound(ResponseDTO.NotFound());
                }

                var models = MapToResponseModels<ResponseModel>(new List<Model>() { entity });
                var resultModel = await FinishMappingResponseModels(models, new List<Model>() { entity }, context);
                return Ok(ResponseDTO.Ok(resultModel.First()));
            }
            catch (Exception e)
            {
                return HandleExceptionInRequest(e);
            }
        }

        protected async Task<IActionResult> _AddItem(Model item)
        {
            IActionResult result = BadRequest(ResponseDTO.BadRequest());
            try
            {
                var insertedItem = await _repository.Add(item);
                result = Ok(ResponseDTO.Ok(insertedItem));
            }
            catch (DbUpdateException ex)
            {
                var res = ResponseDTO.BadRequest($"Cannot add new { typeof(Model).Name }.");
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
                return NotFound(ResponseDTO.NotFound());
            }

            IActionResult response = BadRequest(ResponseDTO.BadRequest());
            if (setFlag)
            {
                item.DeleteFlag = true;
                item = await _repository.Update(item);
                response = Ok(ResponseDTO.OkDeleted(item));
            }
            else
            {
                var result = await _repository.Remove(id);
                response = result
                    ? Ok(ResponseDTO.OkDeleted(item, "Removed from database."))
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


class InvalidQueryParamsException : Exception
{

}