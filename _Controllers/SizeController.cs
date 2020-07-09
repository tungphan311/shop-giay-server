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
    public class SizeController : GeneralController<Size, SizeDTO>
    {
        public SizeController(IAsyncRepository<Size> repo, ILogger<SizeController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        {

        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SizeDTO model)
        {
            // Validate
            if (string.IsNullOrEmpty(model.Name))
            {
                return Ok(ResponseDTO.BadRequest("Not enough information to create."));
            }

            var size = new Size
            {
                Name = model.Name
            };

            return await this._AddItem(size);
        }

        // Size Update
        [Route("admin/[controller]/{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateSize(int id, [FromBody] SizeDTO dto)
        {
            if (id != dto.Id)
            {
                return Ok(ResponseDTO.BadRequest("URL ID and Item ID does not matched."));
            }
            var entity = _mapper.Map<Size>(dto);
            entity.Id = id;

            var updatedItem = await _repository.Update(entity);
            if (updatedItem == null)
            {
                return Ok(ResponseDTO.BadRequest("Item ID is not existed."));
            }
            return Ok(ResponseDTO.Ok(entity));
        }

        protected override async Task<IEnumerable<object>> FinishMappingResponseModels(
            IEnumerable<object> responseEntities,
            IEnumerable<Size> entities,
            RequestContext requestContext)
        {
            try
            {
                switch (requestContext.APIRoute)
                {
                    case APIRoute.ClientGetAll:
                        IEnumerable<SizeDTO> list = (IEnumerable<SizeDTO>)responseEntities;
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
