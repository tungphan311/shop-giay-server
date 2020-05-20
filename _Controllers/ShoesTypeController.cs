using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;

namespace shop_giay_server._Controllers
{
    public class ShoesTypeController : GeneralController<ShoesType>
    {
        public ShoesTypeController(IAsyncRepository<ShoesType> repo, ILogger<ShoesTypeController> logger)
            : base(repo, logger)
        { }
    }
}
