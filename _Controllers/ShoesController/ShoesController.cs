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
using System;

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

        #region Helper Methods


        protected override async Task<IQueryCollection> TransformQuery(IQueryCollection query, RequestContext requestContext)
        {
            switch (requestContext.APIRoute)
            {
                case APIRoute.AdminGetAll:
                case APIRoute.ClientGetAll:
                    query = await AppendGenderQueryIfNeeded(query);
                    query = await AppendNewQueryIfNeeded(query);
                    break;

                default:
                    break;
            }

            return await base.TransformQuery(query, requestContext);
        }


        protected override async Task<IEnumerable<object>> FinishMappingResponseModels(
            IEnumerable<object> responseEntities,
            IEnumerable<Shoes> entities,
            RequestContext requestContext)
        {
            try
            {
                switch (requestContext.APIRoute)
                {
                    case APIRoute.ClientGetByID:
                        var dto = (ResponseShoesDetailDTO)responseEntities.FirstOrDefault();
                        var entity = entities.FirstOrDefault();
                        foreach (var stock in entity.Stocks)
                        {
                            var size = await _context.Sizes.FindAsync(stock.SizeId);
                            if (size != null)
                            {
                                dto.sizes.Add(new
                                {
                                    sizeName = size.Name,
                                    stockId = stock.Id
                                });
                            }
                        }
                        break;

                    case APIRoute.ClientGetAll:

                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Cannot convert to ResponseShoesDetailDTO.");
                return await base.FinishMappingResponseModels(responseEntities, entities, requestContext);
            }

            return await base.FinishMappingResponseModels(responseEntities, entities, requestContext);
        }


        #endregion


        #region Client API

        public override async Task<IActionResult> GetAllForClient()
        {
            return await _GetAllModels<ResponseShoesDTO>(RequestContext.ClientGetAll());
        }


        public override async Task<IActionResult> GetByIdForClient(int id)
        {
            var context = RequestContext.ClientGetByID(id);
            return await _GetById<ResponseShoesDetailDTO>(id, context);
        }

        [Route("client/[controller]/rating")]
        [HttpPost]
        public IActionResult ClientRateShoes([FromBody] BodyShoesRating model)
        {

            return BadRequest();
        }

        #endregion


        #region Admin API

        [Route("admin/[controller]")]
        [HttpPost]
        public async Task<IActionResult> AddShoesForAdmin([FromBody] BodyCreateShoes model)
        {
            // Validate
            if (!model.IsValid())
            {
                return Ok(ResponseDTO.BadRequest("Not enough information to create."));
            }

            if (await isExist(model.Code))
            {
                return Ok(ResponseDTO.BadRequest("Shoes's code is already existed"));
            }

            var images = model.Images;
            Console.WriteLine(model.Images.Count);

            var shoesImages = new List<ShoesImage>();

            foreach (var image in images)
            {
                var shoesImage = new ShoesImage
                {
                    ColorId = image.ColorId,
                    ImagePath = image.ImagePath,
                };
                shoesImages.Add(shoesImage);
            }

            var stocks = new List<Stock>();

            foreach (var item in model.Stocks)
            {
                var s = new Stock
                {
                    ColorId = item.ColorId,
                    SizeId = item.SizeId,
                    Instock = item.Instock,
                };
                stocks.Add(s);
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
                GenderId = model.GenderId,
                ShoesImages = shoesImages,
                Stocks = stocks
            };

            if (shoes.IsNew) shoes.IsNew = true;
            return await this._AddItem(shoes);
        }


        [Route("admin/[controller]/{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateShoes(int id, [FromBody] ShoesDTO dto)
        {
            if (id == dto.Id) 
            {
                return Ok(ResponseDTO.BadRequest("URL ID and Item ID does not matched."));
            }
            var entity = _mapper.Map<Shoes>(dto);
            entity.Id = id;

            var updatedItem = await _repository.Update(entity);
            if (updatedItem == null) 
            {
                return Ok(ResponseDTO.BadRequest("Item ID is not existed."));
            }

            return Ok(ResponseDTO.Ok(entity));
        }

        #endregion


        #region Query Params Modifiers 

        private async Task<IQueryCollection> AppendGenderQueryIfNeeded(IQueryCollection query)
        {
            var genderKey = "gender";
            if (query[genderKey].Count == 1)
            {
                var paramValue = query[genderKey][0];
                var genderEntity = await _context.Genders.FirstOrDefaultAsync(g => g.Name.ToLower() == paramValue.ToString().ToLower());

                if (genderEntity != null)
                {
                    var queryItem = new KeyValuePair<String, StringValues>("genderId", genderEntity.Id.ToString());
                    query = query.AppendQueryItem(queryItem);
                }
                
            }
            return query;
        }

        private async Task<IQueryCollection> AppendNewQueryIfNeeded(IQueryCollection query)
        {
            var isNewKey = "new";
            if (query[isNewKey].Count == 1)
            {
                var queryParam = query[isNewKey];
                var queryItem = new KeyValuePair<String, StringValues>("isNew", queryParam[0]);
                query = query.AppendQueryItem(queryItem);
            }
            return await Task.FromResult(query);
        }

        #endregion


        #region Private Methods

        private async Task<bool> isExist(string code)
        {
            if (await _context.Shoes.AnyAsync(s => s.Code == code))
            {
                return true;
            }
            return false;
        }

        #endregion


    }
}
