using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using shop_giay_server.data;
using AutoMapper;

namespace shop_giay_server._Controllers
{
    public class CustomerTypeController : GeneralController<CustomerType, CustomerTypeDTO>
    {
        public CustomerTypeController(IAsyncRepository<CustomerType> repo, ILogger<CustomerTypeController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        { }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerTypeDTO model)
        {
            // Validate
            if (string.IsNullOrEmpty(model.Name))
            {
                return Ok(ResponseDTO.BadRequest("Not enough information to create."));
            }

            var item = _mapper.Map<CustomerType>(model);
            return await this._AddItem(item);
        }
    }
}
