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

        public OrderController(IAsyncRepository<Order> repo, ILogger<OrderController> logger, IMapper mapper, DataContext context)
            : base(repo, logger, mapper, context)
        {
        }

        #region Admin's API

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
                orderItem.ShoesId = shoesInfo.id;
            }
        }


        private (int id, string name, string imagePath) GetShoesInfoByStockId(int stockId)
        {
            var name = "";
            var imagePath = "";
            var id = 0;
            var stock = _context.Stocks
                .Include(c => c.Shoes)
                .ThenInclude(c => c.ShoesImages)
                .FirstOrDefault(o => o.Id == stockId);
            if (stock.Shoes != null)
            {
                if (!String.IsNullOrEmpty(stock.Shoes.Name)) { name = stock.Shoes.Name; id = stock.Shoes.Id; };

                var shoesImage = stock.Shoes.ShoesImages.FirstOrDefault();
                if (!String.IsNullOrEmpty(shoesImage.ImagePath)) imagePath = shoesImage.ImagePath;
            }
            return (id, name, imagePath);
        }

        // Update Order
        [Route("admin/[controller]/{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateOrderDTO dto)
        {
            if (id != dto.Id)
            {
                return Ok(ResponseDTO.BadRequest("URL ID and Item ID does not matched."));
            }

            // Validate
            var entity = await _repository.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return Ok(ResponseDTO.BadRequest("Item ID is not existed."));
            }

            entity.Status = dto.Status;
            entity.ConfirmDate = dto.ConfirmDate;
            entity.CancelDate = dto.CancelDate;
            entity.BeginDelivery = dto.BeginDelivery;
            entity.DeliveryDate = dto.DeliveryDate;
            entity.Note = dto.Note;

            await _repository.Update(entity);

            return Ok(ResponseDTO.Ok(entity));
        }

        #endregion

        #region Client's API

        [HttpPost]
        [Route("client/[controller]/{addressId:int}")]
        public async Task<IActionResult> ClientProcessOrder(int addressId)
        {
            var customer = GetCustomer();
            if (customer == null)
            {
                return Ok(ResponseDTO.BadRequest("Invalid customer's username."));
            }



            return Ok();
        }

        #endregion

        #region HELPER METHODS

        public async Task<Order> CreateOrder(int customerId)
        {
            // get cart items
            return null;
        }

        public Customer GetCustomer()
        {
            var sessionUsername = HttpContext.Session.GetString(SessionConstant.Username);
            if (string.IsNullOrEmpty(sessionUsername))
            {
                return null;
            }

            var customer = _context.Customers.FirstOrDefault(c => c.Username == sessionUsername);
            return customer;
        }

        #endregion

    }

    public class UpdateOrderDTO
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? BeginDelivery { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string Note { get; set; }
    }

    public class ClientOrderResponseDTO
    {
        public int id { get; set; }
        public int customerID { get; set; }
        public int saleID { get; set; }
        public string city { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime confirmDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public float total { get; set; }
        public int status { get; set; }
        public int paymentStatus { get; set; }
        public int deliveryAddress { get; set; }
        public int recipientName { get; set; }
        public int recipientPhoneNumber { get; set; }
        public List<ClientOrder_CartItemDTO> cartItemDTOList { get; set; } = new List<ClientOrder_CartItemDTO>();
    }

    public class ClientOrder_CartItemDTO
    {
        public int stockId { get; set; }
        public int shoesId { get; set; }
        public string name { get; set; }
        public string sizeName { get; set; }
        public string quantity { get; set; }
        public float price { get; set; }
        public string image { get; set; }
    }
}
