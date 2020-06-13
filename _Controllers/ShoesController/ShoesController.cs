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

        #region HELPER METHODS


        protected override async Task<IQueryCollection> TransformQuery(IQueryCollection query, RequestContext requestContext)
        {
            switch (requestContext.APIRoute)
            {
                case APIRoute.AdminGetAll:
                case APIRoute.ClientGetAll:
                    var genderKey = "gender";
                    if (query[genderKey].Count == 1)
                    {
                        var paramItem = query[genderKey];
                        var genderEntity = await _context.Genders.FirstAsync(g => g.Name == paramItem.ToString());
                        var queryItem = new KeyValuePair<String, StringValues>("genderId", genderEntity.Id.ToString());
                        query.Append(queryItem);
                    }

                    var isNewKey = "new";
                    if (query[isNewKey].Count == 1)
                    {
                        var queryParam = query[isNewKey];
                        var queryItem = new KeyValuePair<String, StringValues>("isNew", queryParam[0]);
                        query.Append(queryItem);
                    }

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

            switch (requestContext.APIRoute)
            {
                case APIRoute.ClientGetByID:
                    try
                    {
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

                        return await base.FinishMappingResponseModels(responseEntities, entities, requestContext);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(0, e, "Cannot convert to ResponseShoesDetailDTO.");
                        return await base.FinishMappingResponseModels(responseEntities, entities, requestContext);
                    }
                default:
                    break;
            }

            return await base.FinishMappingResponseModels(responseEntities, entities, requestContext);
        }


        #endregion


        #region CLIENT API

        public override async Task<IActionResult> GetAllForClient()
        {
            return await _GetAllModels<ResponseShoesDTO>(RequestContext.ClientGetAll());
        }


        public override async Task<IActionResult> GetByIdForClient(int id)
        {
            var context = RequestContext.ClientGetByID(id);
            return await _GetById<ResponseShoesDetailDTO>(id, context);
        }

        #endregion


        #region ADMIN API

        [Route("admin/[controller]")]
        [HttpPost]
        public async Task<IActionResult> AddShoesForAdmin([FromBody] CreateShoesBody model)
        {
            // Validate
            if (!model.IsValid())
            {
                return BadRequest(ResponseDTO.BadRequest("Not enough information to create."));
            }

            if (await isExist(model.Code))
            {
                return BadRequest(ResponseDTO.BadRequest("Shoes's code is already existed"));
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
            if (!(await _repository.ExistWhere(c => c.Id == id)))
            {
                return BadRequest(ResponseDTO.BadRequest("Item ID is not existed."));
            }
            var entity = _mapper.Map<Shoes>(dto);
            var updatedItem = await _repository.Update(entity);
            return Ok(ResponseDTO.Ok(entity));
        }

        #endregion


        public async Task<bool> isExist(string code)
        {
            if (await _context.Shoes.AnyAsync(s => s.Code == code))
            {
                return true;
            }
            return false;
        }




    }
}
