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
    }
}
