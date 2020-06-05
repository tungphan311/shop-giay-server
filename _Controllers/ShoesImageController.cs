using AutoMapper;
using Microsoft.Extensions.Logging;
using shop_giay_server._Repository;
using shop_giay_server.Dtos;
using shop_giay_server.models;

namespace shop_giay_server._Controllers
{
    public class ShoesImageController: GeneralController<ShoesImage, ShoesImageDTO>
    {
        public ShoesImageController(IAsyncRepository<ShoesImage> repo, ILogger<ShoesImageController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        {
            
        }
    }
}