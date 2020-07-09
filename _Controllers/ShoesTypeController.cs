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
using System.Linq.Dynamic.Core;
using System;

namespace shop_giay_server._Controllers
{
    public class ShoesTypeController : GeneralController<ShoesType, ShoesTypeDTO>
    {
        public ShoesTypeController(IAsyncRepository<ShoesType> repo, ILogger<ShoesTypeController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ShoesTypeDTO model)
        {
            // Validate
            if (string.IsNullOrEmpty(model.Name))
            {
                return Ok(ResponseDTO.BadRequest("Not enough information to create."));
            }

            var item = _mapper.Map<ShoesType>(model);
            return await this._AddItem(item);
        }

        [Route("client/types")]
        [HttpGet]
        public async virtual Task<IActionResult> GetAllWithCustomRoute()
        {
            var context = RequestContext.ClientGetAll();
            return await _GetAllModels<ShoesTypeDTO>(context);
        }

        protected override async Task<IEnumerable<object>> FinishMappingResponseModels(
            IEnumerable<object> responseEntities,
            IEnumerable<ShoesType> entities,
            RequestContext requestContext)
        {
            try
            {
                switch (requestContext.APIRoute)
                {
                    case APIRoute.ClientGetAll:
                        IEnumerable<ShoesTypeDTO> list = (IEnumerable<ShoesTypeDTO>)responseEntities;
                        responseEntities = list.Select(c => c.Name);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Cannot convert.");
                return await base.FinishMappingResponseModels(responseEntities, entities, requestContext);
            }

            return await base.FinishMappingResponseModels(responseEntities, entities, requestContext);
        }
    }
}
