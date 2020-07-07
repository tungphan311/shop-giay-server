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
    public class SaleController : GeneralController<Sale, SaleDTO>
    {
        private readonly IAsyncRepository<Shoes> _shoesRepo;
        public SaleController(IAsyncRepository<Sale> repo, IAsyncRepository<Shoes> shoesRepo, ILogger<SaleController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        {
            _shoesRepo = shoesRepo;
        }

        public async Task<bool> validAddRequest(SaleDTO dto)
        {
            if (dto.SaleType < 0 || dto.SaleType > 2) return false;
            if (dto.SaleType == 1 && (dto.Amount < 0 || dto.Amount > 100)) return false;

            if (dto.BeginDate == null || dto.ExpiredDate == null || dto.SaleProducts == null) return false;

            if (dto.ExpiredDate <= dto.BeginDate) return false;

            foreach (var product in dto.SaleProducts)
            {
                var shoes = await _shoesRepo.FirstOrDefault(s => s.Id == product.ShoesId);

                if (shoes == null || shoes.IsOnSale == true)
                {
                    return false;
                }
            }

            return true;
        }

        [Route("admin/[controller]")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SaleDTO dto)
        {
            // Validate request
            if (!(await validAddRequest(dto)))
            {
                return Ok(ResponseDTO.BadRequest("Thông tin không hợp lệ"));
            }

            foreach (var product in dto.SaleProducts)
            {
                var shoes = await _shoesRepo.FirstOrDefault(s => s.Id == product.ShoesId);
                shoes.IsOnSale = true;

                await _shoesRepo.Update(shoes);
            }

            var sale = _mapper.Map<Sale>(dto);
            return await this._AddItem(sale);
        }
    }
}
