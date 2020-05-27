﻿using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using System.Linq;

namespace shop_giay_server._Controllers
{
    public class SizeController : GeneralController<Size, SizeDTO>
    {
        public SizeController(IAsyncRepository<Size> repo, ILogger<SizeController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SizeDTO model)
        {
            // Validate
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest(Response<ShoesType>.BadRequest("Not enough information to create."));
            }

            var size = new Size
            {
                Name = model.Name
            };

            return await this.AddItem(size);
        }
    }
}
