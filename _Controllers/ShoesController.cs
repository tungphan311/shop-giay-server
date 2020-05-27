using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;

namespace shop_giay_server._Controllers
{
    public class ShoesController : GeneralController<Shoes, ShoesDTO>
    {
        public ShoesController(IAsyncRepository<Shoes> repo, ILogger<ShoesController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        { }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateShoesBody model)
        {
            // Validate
            if (!model.IsValid())
            {
                return BadRequest(Response<Shoes>.BadRequest("Not enough information to create."));
            }

            var shoes = new Shoes()
            {
                Code = model.Code,
                Name = model.Name,
                Description = model.Description,
                Rating = model.Rating,
                Price = model.Price,
                IsNew = model.IsNew,
                IsOnSale = model.IsOnSale,
                StyleId = model.StyleId,
                BrandId = model.BrandId,
                GenderId = model.GenderId
            };

            if (shoes.IsNew) shoes.IsNew = true;
            return await this.AddItem(shoes);
        }

    }

    public class CreateShoesBody
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public float Price { get; set; }
        public bool IsNew { get; set; }
        public bool IsOnSale { get; set; }
        public int StyleId { get; set; }
        public int BrandId { get; set; }
        public int GenderId { get; set; }

        public bool IsValid()
        {
            return Code != null
                && Name != null
                && Price > 0
                && StyleId > 0
                && BrandId > 0
                && GenderId > 0;
        }
    }
}
