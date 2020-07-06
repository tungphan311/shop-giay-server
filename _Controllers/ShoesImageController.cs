using AutoMapper;
using Microsoft.Extensions.Logging;
using shop_giay_server._Repository;
using shop_giay_server.Dtos;
using shop_giay_server.models;
using shop_giay_server.data;

namespace shop_giay_server._Controllers
{
    public class ShoesImageController: GeneralController<ShoesImage, ShoesImageDTO>
    {
        public ShoesImageController(IAsyncRepository<ShoesImage> repo, ILogger<ShoesImageController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        {
            
        }
    }
}