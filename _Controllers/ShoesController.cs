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
using System.Collections.Generic;
using shop_giay_server.data;
using Microsoft.EntityFrameworkCore;

namespace shop_giay_server._Controllers
{
    public class ShoesController : GeneralController<Shoes, ShoesDTO>
    {
        private DataContext _context;


        public ShoesController(
            IAsyncRepository<Shoes> repo,
            DataContext context,
            ILogger<ShoesController> logger,
            IMapper mapper)
            : base(repo, logger, mapper)
        {
            _context = context;
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
                    var genderModel = await _context.Genders.FirstAsync(g => g.Name == gender.ToString());
                    dict["genderId"] = genderModel.Id.ToString();
                }
            }

            StringValues isNewShoes = "";
            if (dict.TryGetValue("new", out isNewShoes))
            {
                if (isNewShoes != "")
                {
                    dict["isNew"] = isNewShoes; 
                }
            }

            return await _GetAllForClient<ResponseShoesDTO>(dict);
        }


        protected override async Task<IActionResult> _GetForClient<DTO>(int id)
        {

            IActionResult actionResult = NotFound(Response<Shoes>.NotFound());
            var item = await _repository.GetById(id);
            var result = _mapper.Map<ResponseShoesDetailDTO>(item);

            foreach (var stock in item.Stocks)
            {
                var size = await _context.Sizes.FindAsync(stock.SizeId);
                if (size != null)
                {
                    result.sizes.Add(size.Name);
                }
            }

            if (item != null)
            {
                actionResult = Ok(Response<ResponseShoesDetailDTO>.Ok(result));
            }

            return actionResult;
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

    public class ResponseShoesDTO : BaseDTO
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

    public class ResponseShoesDetailDTO : BaseDTO
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float rating { get; set; }
        public float ratingCount { get; set; }
        public string styleName { get; set; }
        public string brandName { get; set; }
        public string genderName { get; set; }
        public int price { get; set; }
        public int isNew { get; set; }
        public int isOnSale { get; set; }
        public int salePrice { get; set; }
        public List<string> images { get; set; } = new List<string>();

        [IgnoreMap]
        public List<string> sizes { get; set; } = new List<string>();
    }
}
