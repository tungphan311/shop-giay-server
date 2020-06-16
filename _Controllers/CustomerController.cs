using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;

namespace shop_giay_server._Controllers
{
    public class CustomController : GeneralController<Customer, CustomerDTO>
    {
        public CustomController(IAsyncRepository<Customer> repo, ILogger<CustomController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerDTO model)
        {
            // Validate
            var existed = await _repository.ExistWhere(c => c.Email == model.Email
                    || c.PhoneNumber == model.PhoneNumber
                    || c.Username == model.Username);

            if (existed)
            {
                return BadRequest(ResponseDTO.BadRequest("Customer has been existed."));
            }


            var item = _mapper.Map<Customer>(model);
            return await this._AddItem(item);
        }

        [HttpPost]
        [Route("client/customer/getInfo")]
        public async Task<IActionResult> GetInfo() 
        {
            return BadRequest();
        }

        #region Helper Methods

        #endregion
    }
}
