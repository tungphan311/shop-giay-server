﻿using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using shop_giay_server.data;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace shop_giay_server._Controllers
{
    public class ImportController : GeneralController<Import, ImportDTO>
    {
        private IAsyncRepository<Stock> _stockRepository;

        public ImportController(IAsyncRepository<Import> repo, IAsyncRepository<Stock> stockRepo, ILogger<ImportController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        {
            _stockRepository = stockRepo;
        }

        [Route("admin/[controller]")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateImport model)
        {
            var details = model.Details;

            // Validate
            for (int i = 0; i < details.Count; i++)
            {
                var id = details[i].StockId;
                var stock = await _stockRepository.FirstOrDefault(c => c.Id == id);

                if (stock == null)
                {
                    return Ok(ResponseDTO.BadRequest($"Not found stockId: {id}"));
                }
                else
                {
                    stock.Instock += details[i].Quantity;
                    await _stockRepository.Update(stock);
                }
            }

            float totalCost = 0;
            int totalQuantity = 0;

            foreach (var detail in details)
            {
                totalCost += detail.Quantity * detail.OriginalPrice;
                totalQuantity += detail.Quantity;
            }

            var importDetails = new List<ImportDetail>();
            foreach (var detail in details)
            {
                var importDetail = new ImportDetail
                {
                    Quantity = detail.Quantity,
                    OriginalPrice = detail.OriginalPrice,
                    StockId = detail.StockId,
                };

                importDetails.Add(importDetail);
            }

            var import = new Import
            {
                ImportDate = DateTime.Now,
                TotalQuantity = totalQuantity,
                TotalCost = totalCost,
                ProviderId = model.ProviderId,
                ImportDetails = importDetails
            };


            return await this._AddItem(import);
        }
    }


    #region Body Request models

    public class CreateImport
    {
        public bool DeleteFlag { get; set; } = false;
        public int ProviderId { get; set; }
        public List<ImportDetailDTO> Details { get; set; } = new List<ImportDetailDTO>();
    }

    #endregion
}
