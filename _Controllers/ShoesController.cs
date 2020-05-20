using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;

namespace shop_giay_server._Controllers
{
    public class ShoesController : GeneralController<Shoes>
    {
        public ShoesController(IAsyncRepository<Shoes> repo, ILogger<ShoesController> logger)
            : base(repo, logger)
        { }
    }
}
