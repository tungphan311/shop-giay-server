using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;

namespace shop_giay_server._Controllers
{
    public class ColorController : GeneralController<Color, ColorDTO>
    {
        public ColorController(IAsyncRepository<Color> repo, ILogger<ColorController> logger)
            : base(repo, logger)
        { }
    }
}
