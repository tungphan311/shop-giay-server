using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using shop_giay_server.data;
using AutoMapper;
using System;

namespace shop_giay_server._Controllers
{
    public class CustomerReviewController : GeneralController<CustomerReview, CustomerReviewDTO>
    {
        public CustomerReviewController(IAsyncRepository<CustomerReview> repo, ILogger<CustomerReviewController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerReviewDTO model)
        {
            // Validate
            if (model.Content.Length > 0 && model.Rate >= 0 && model.Rate <= 10)
            {
                return Ok(ResponseDTO.BadRequest("Invalid content or rating."));
            }


            var item = _mapper.Map<CustomerReview>(model);
            item.Date = DateTime.Now;
            return await this._AddItem(item);
        }
    }
}
