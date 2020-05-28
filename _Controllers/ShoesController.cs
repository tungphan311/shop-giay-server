using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace shop_giay_server._Controllers
{
    public class ShoesController : GeneralController<Shoes, ShoesDTO>
    {
        private IAsyncRepository<Gender> _genderRepo;


        public ShoesController(IAsyncRepository<Shoes> repo, IAsyncRepository<Gender> genderRepo, ILogger<ShoesController> logger, IMapper mapper)
            : base(repo, logger, mapper)
        {
            _genderRepo = genderRepo;
        }

        public override async Task<IActionResult> GetAllForClient()
        {
            var dict = this.Request.Query.ToDictionary(
                p => p.Key.ToLower(),
                p => p.Value);

            StringValues gender = "";
            if (dict.TryGetValue("gender", out gender))
            {
                if (gender != "")
                {
                    var genderModel = _genderRepo.GetWhere(g => g.Name == gender.ToString());
                    dict["genderId"] = genderModel.Id.ToString();
                }
            }

            return await _GetAllForClient<ReponseShoesDTO>(dict);
        }

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

    public class ReponseShoesDTO : BaseDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public int isNew { get; set; }
        public int isOnSale { get; set; }
        public string imagePath { get; set; }
        public int quantity { get; set; }
        public int salePrice { get; set; }
        public string styleName { get; set; }
    }
}
