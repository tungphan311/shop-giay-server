﻿using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using System;

namespace shop_giay_server._Controllers
{
    public class CustomerReviewController : GeneralController<CustomerReview, CustomerReviewDTO>
    {
        public CustomerReviewController(IAsyncRepository<CustomerReview> repo, ILogger<CustomerReviewController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerReviewDTO model)
        {
            // Validate
            if (model.Content.Length > 0 && model.Rate >= 0 && model.Rate <= 10)
            {
                return BadRequest(Response<CustomerReview>.BadRequest("Invalid content or rating."));
            }


            var item = _mapper.Map<CustomerReview>(model);
            item.Date = DateTime.Now;
            return await this._AddItem(item);
        }
    }
}
