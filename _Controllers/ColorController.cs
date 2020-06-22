using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;

namespace shop_giay_server._Controllers
{
    public class ColorController : GeneralController<Color, ColorDTO>
    {
        public ColorController(IAsyncRepository<Color> repo, ILogger<ColorController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ColorDTO model)
        {
            // Validate
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest(ResponseDTO.BadRequest("Not enough information to create."));
            }

            var color = new Color
            {
                Name = model.Name
            };

            return await this._AddItem(color);
        }
        // Customer Update
        [Route("admin/[controller]/{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDTO dto)
        {
            if (id == dto.Id) 
            {
                return BadRequest(ResponseDTO.BadRequest("URL ID and Item ID does not matched."));
            }
            var entity = _mapper.Map<Customer>(dto);
            entity.Id = id;

            var updatedItem = await _repository.Update(entity);
            if (updatedItem == null) 
            {
                return BadRequest(ResponseDTO.BadRequest("Item ID is not existed."));
            }
            return Ok(ResponseDTO.Ok(entity));
        }
    }
}
