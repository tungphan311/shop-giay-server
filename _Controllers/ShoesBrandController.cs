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
    public class ShoesBrandController : GeneralController<ShoesBrand, ShoesBrandDTO>
    {
        public ShoesBrandController(IAsyncRepository<ShoesBrand> repo, ILogger<ShoesBrandController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ShoesBrandDTO model)
        {
            // Validate
            if (string.IsNullOrEmpty(model.Name))
            {
                return Ok(ResponseDTO.BadRequest("Not enough information to create."));
            }

            var item = _mapper.Map<ShoesBrand>(model);
            return await this._AddItem(item);
        }

        [Route("client/brands")]
        [HttpGet]
        public async virtual Task<IActionResult> GetAllWithCustomRoute()
        {
            var context = RequestContext.ClientGetAll();
            return await _GetAllModels<ShoesBrandDTO>(context);
        }

        protected override async Task<IEnumerable<object>> FinishMappingResponseModels(
            IEnumerable<object> responseEntities,
            IEnumerable<ShoesBrand> entities,
            RequestContext requestContext)
        {
            try
            {
                switch (requestContext.APIRoute)
                {
                    case APIRoute.ClientGetAll:
                        IEnumerable<ShoesBrandDTO> list = (IEnumerable<ShoesBrandDTO>)responseEntities;
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
