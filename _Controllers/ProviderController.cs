using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using System;

namespace shop_giay_server._Controllers
{
    public class ProviderController : GeneralController<Provider, ProviderDTO>
    {
        public ProviderController(IAsyncRepository<Provider> repo, ILogger<ProviderController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProviderForCreateDTO dto)
        {
            var provideForCreate = new Provider
            {
                Name = dto.Name
            };

            var item = await _repository.Add(provideForCreate);
            var res = new Response<Provider>(item);
            
            return Ok(res);
        }
    }
}
