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
using System.Linq.Dynamic.Core;
using System;

namespace shop_giay_server._Controllers
{
    public class ShoesController : GeneralController<Shoes, ShoesDTO>
    {

        public ShoesController(
            IAsyncRepository<Shoes> repo,
            DataContext context,
            ILogger<ShoesController> logger,
            IMapper mapper)
            : base(repo, logger, mapper, context)
        { }

        #region Helper Methods

        protected override async Task<IQueryCollection> TransformQuery(IQueryCollection query, RequestContext requestContext)
        {
            switch (requestContext.APIRoute)
            {
                case APIRoute.AdminGetAll:
                case APIRoute.ClientGetAll:
                    query = AppendPriceQueryIfNeeded(query);
                    query = AppendQueryByNameIfNeeded(query);
                    query = AppendQueryByMappKeysIfNeeded(query);

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
                        var saleInfo = GetSaleInfo(dto.id);
                        dto.isOnSale = saleInfo.isOnSale;
                        dto.salePrice = saleInfo.salePrice;
                        // var saleInfo = _context.SaleProducts
                        //                         .Include(c => c.Sale)
                        //                         .FirstOrDefault(c => c.ShoesId == dto.id);
                        // if (saleInfo != null && saleInfo.Sale.Status != 0)
                        // {
                        //     var sale = saleInfo.Sale;
                        //     dto.isOnSale = true;
                        //     dto.salePrice = sale.SaleType == 1
                        //         ? dto.price * (float)(1 - (float)sale.Amount / 100.0)
                        //         : dto.price - sale.Amount;
                        // }
                        // else
                        // {
                        //     dto.isOnSale = false;
                        //     dto.salePrice = dto.price;
                        // }
                        break;


                    case APIRoute.ClientGetAll:
                        foreach (ResponseShoesDTO resEntity in responseEntities)
                        {
                            var sInfo = GetSaleInfo(resEntity.id);
                            resEntity.isOnSale = sInfo.isOnSale;
                            resEntity.salePrice = sInfo.salePrice;
                            // var saleProduct = _context.SaleProducts
                            //                     .Include(c => c.Sale)
                            //                     .FirstOrDefault(c => c.ShoesId == resEntity.id);
                            // if (saleProduct != null && saleProduct.Sale.Status != 0)
                            // {
                            //     var sale = saleProduct.Sale;
                            //     resEntity.isOnSale = true;
                            //     resEntity.salePrice = sale.SaleType == 1
                            //         ? resEntity.price * (float)(1 - (float)sale.Amount / 100.0)
                            //         : resEntity.price - sale.Amount;
                            // }
                            // else
                            // {
                            //     resEntity.isOnSale = false;
                            //     resEntity.salePrice = resEntity.price;
                            // }
                        }
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


        private (float salePrice, bool isOnSale) GetSaleInfo(int shoesId)
        {
            var shoes = _context.Shoes.FirstOrDefault(c => c.Id == shoesId);
            if (shoes == null)
            {
                return (0, false);
            }

            var saleInfo = _context.SaleProducts
                                .Include(c => c.Sale)
                                .FirstOrDefault(c => c.ShoesId == shoesId);
            if (saleInfo != null && saleInfo.Sale.Status != 0)
            {
                var sale = saleInfo.Sale;
                var salePrice = sale.SaleType == 1
                    ? shoes.Price * (float)(1 - (float)sale.Amount / 100.0)
                    : shoes.Price - sale.Amount;
                return (salePrice, true);
            }
            else
            {
                return (shoes.Price, false);
            }
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
        public async Task<IActionResult> ClientRateShoes([FromBody] BodyShoesRating model)
        {
            // Check customer
            var customer = GetCustomer();
            if (customer == null)
            {
                return Ok(ResponseDTO.BadRequest("Login required."));
            }

            // Check shoes
            var shoes = await _context.Shoes
                .Include(c => c.ShoesBrand)
                .Include(c => c.Gender)
                .Include(c => c.ShoesType)
                .Include(c => c.ShoesImages)
                .Include(c => c.Stocks).ThenInclude(c => c.Size)
                .FirstOrDefaultAsync(c => c.Id == model.shoesId);

            if (shoes == null)
            {
                return Ok(ResponseDTO.BadRequest("Invalid shoesId."));
            }

            if (model.rating < 0) model.rating = 0;
            if (model.rating > 5) model.rating = 5;

            var review = new CustomerReview()
            {
                ShoesId = shoes.Id,
                CustomerId = customer.Id,
                Rate = model.rating,
                Content = "",
                Date = DateTime.Now
            };
            _context.CustomerReviews.Add(review);
            await _context.SaveChangesAsync();

            // Get rating
            var ratingInfo = GetRatingForShoes(shoes.Id);

            // Get sale
            var saleInfo = GetSaleInfo(shoes.Id);

            List<object> sizes = (List<object>)shoes.Stocks.Select(c => (object)c.Size.Name).ToList();

            var result = new ResponseShoesDetailDTO()
            {
                id = shoes.Id,
                code = shoes.Code,
                name = shoes.Name,
                description = shoes.Description,
                rating = ratingInfo.rating,
                ratingCount = ratingInfo.ratingCount,
                styleName = shoes.ShoesType.Name,
                brandName = shoes.ShoesBrand.Name,
                genderName = shoes.Gender.Name,
                price = shoes.Price,
                isNew = shoes.IsNew,
                isOnSale = saleInfo.isOnSale,
                salePrice = saleInfo.salePrice,
                images = shoes.ShoesImages.Select(c => c.ImagePath).ToList(),
                sizes = sizes
            };

            return Ok(ResponseDTO.Ok(result));
        }


        private (float rating, int ratingCount) GetRatingForShoes(int shoesId)
        {
            var shoes = _context.Shoes.FirstOrDefault(c => c.Id == shoesId);
            if (shoes == null)
            {
                return (0, 0);
            }

            var listReview = _context.CustomerReviews.Where(c => c.ShoesId == shoesId).Select(c => c.Rate);
            var totalRate = (float)listReview.Sum() / (float)listReview.Count();
            return (totalRate, listReview.Count());
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

        // Customer update + Address Update clone cai nay
        public async Task<IActionResult> UpdateShoes(int id, [FromBody] ShoesDTO dto)
        {
            if (id != dto.Id)
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

        private IQueryCollection AppendPriceQueryIfNeeded(IQueryCollection query)
        {
            var key = "price-range";
            if (query[key].Count == 1)
            {
                var value = query[key][0].Replace(" ", "");

                if (value.Contains("-"))
                {
                    var values = value.Split("-");
                    if (values.Count() != 2) return query;
                    var minPrice = new KeyValuePair<String, StringValues>("minprice", values[0].ToString());
                    var maxPrice = new KeyValuePair<String, StringValues>("maxprice", values[1].ToString());
                    query = query.AppendQueryItem(minPrice);
                    query = query.AppendQueryItem(maxPrice);
                }
                else if (value.Contains("<"))
                {
                    var maxValue = value.Replace("<", "");
                    var maxPrice = new KeyValuePair<String, StringValues>("maxprice", maxValue);
                    query = query.AppendQueryItem(maxPrice);
                }
                else if (value.Contains(">"))
                {
                    var minValue = value.Replace(">", "");
                    var minPrice = new KeyValuePair<String, StringValues>("minprice", minValue);
                    query = query.AppendQueryItem(minPrice);
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

        private IQueryCollection AppendQueryByNameIfNeeded(IQueryCollection query)
        {
            var queryKeyInfos = new List<(string queryKey, string internalKey, Type type)>() {
                        (queryKey: "brand", internalKey: "brandId", type: typeof(ShoesBrand)),
                        (queryKey: "size", internalKey: "sizeId", type: typeof(Size)),
                        (queryKey: "style", internalKey: "styleId", type: typeof(ShoesType)),
                        (queryKey: "gender", internalKey: "genderId", type: typeof(Gender))
                    };
            foreach (var info in queryKeyInfos)
            {
                if (query[info.queryKey].Count == 1)
                {
                    var paramValue = query[info.queryKey][0];
                    // var entity = await _context.Genders.FirstOrDefaultAsync(g => g.Name.ToLower() == paramValue.ToString().ToLower());
                    // var type = typeof(Gender);
                    //dynamic entity = _context.Query(info.type).Where("o => o.Name == @0", paramValue);
                    dynamic entity = _context.Query(info.type).FirstOrDefault("c => c.Name == @0", paramValue);

                    var id = 0;
                    if (entity != null)
                    {
                        id = entity.Id;
                    }
                    var queryItem = new KeyValuePair<String, StringValues>(info.internalKey, id.ToString());
                    query = query.AppendQueryItem(queryItem);
                }
            }
            return query;
        }

        public IQueryCollection AppendQueryByMappKeysIfNeeded(IQueryCollection query)
        {
            var plainMaps = new Dictionary<String, String>() {
                        { "new", "isNew" },
                        { "sale", "isOnSale" },
                    };
            foreach (var kv in plainMaps)
            {
                if (query[kv.Key].Count == 1)
                {
                    var queryParam = query[kv.Key];
                    var queryItem = new KeyValuePair<String, StringValues>(kv.Value, queryParam[0]);
                    query = query.AppendQueryItem(queryItem);
                }
            }
            return query;
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

        public Customer GetCustomer()
        {
            var sessionUsername = HttpContext.Session.GetString(SessionConstant.Username);
            if (string.IsNullOrEmpty(sessionUsername))
            {
                return null;
            }

            var customer = _context.Customers
                .Include(c => c.Addresses)
                .Include(c => c.Cart).ThenInclude(c => c.CartItems).ThenInclude(c => c.Stock).ThenInclude(c => c.Shoes).ThenInclude(c => c.ShoesImages)
                .Include(c => c.Cart).ThenInclude(c => c.CartItems).ThenInclude(c => c.Stock).ThenInclude(c => c.Size)
                .Include(c => c.Orders).ThenInclude(c => c.OrderItems)
                .FirstOrDefault(c => c.Username == sessionUsername);
            return customer;
        }

        #endregion


    }
}
