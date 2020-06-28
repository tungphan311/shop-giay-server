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
    public class OrderController : GeneralController<Order, OrderDTO>
    {

        private DataContext _context;

        public OrderController(IAsyncRepository<Order> repo, ILogger<OrderController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper)
        {
            _context = context;
        }


        protected override async Task<IEnumerable<object>> FinishMappingResponseModels(
            IEnumerable<object> responseEntities,
            IEnumerable<Order> entities,
            RequestContext requestContext)
        {
            switch (requestContext.APIRoute)
            {
                case APIRoute.AdminGetAll:
                    foreach (OrderDTO order in responseEntities)
                    {
                        CustomizeOrderItem(order);
                    }
                    break;

                case APIRoute.AdminGetByID:
                    var dto = (OrderDTO)responseEntities.FirstOrDefault();
                    CustomizeOrderItem(dto);
                    break;

                default:
                    break;
            }

            return await base.FinishMappingResponseModels(responseEntities, entities, requestContext);
        }


        private void CustomizeOrderItem(OrderDTO dto)
        {
            foreach (OrderItemLiteDTO orderItem in dto.OrderItems)
            {
                var shoesInfo = GetShoesInfoByStockId(orderItem.StockId);
                orderItem.ShoesName = shoesInfo.name;
                orderItem.ImagePath = shoesInfo.imagePath;
            }
        }


        private (string name, string imagePath) GetShoesInfoByStockId(int stockId)
        {
            var name = "";
            var imagePath = "";
            var stock = _context.Stocks
                .Include(c => c.Shoes)
                .ThenInclude(c => c.ShoesImages)
                .FirstOrDefault(o => o.Id == stockId);
            if (stock.Shoes != null)
            {
                if (!String.IsNullOrEmpty(stock.Shoes.Name)) name = stock.Shoes.Name;

                var shoesImage = stock.Shoes.ShoesImages.FirstOrDefault();
                if (!String.IsNullOrEmpty(shoesImage.ImagePath)) imagePath = shoesImage.ImagePath;
            }
            return (name, imagePath);
        }
    }
}
