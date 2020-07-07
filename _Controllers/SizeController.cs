using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using shop_giay_server.data;
using AutoMapper;
using System.Linq;

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
    }
}
