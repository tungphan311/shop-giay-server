using System;
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class OrderLiteDTO : BaseDTO
    {
        public DateTime OrderDate { get; set; }

        public float Total { get; set; }

        public int Status { get; set; }

        public string DeliverAddress { get; set; }

        public DateTime? ConfirmDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string RecipientName { get; set; }

        public string RecipientPhoneNumber { get; set; }

        public int CustomerId { get; set; }
    }

    public class OrderDTO : OrderLiteDTO
    {
        public CustomerLiteDTO Customer { get; set; }

        public List<OrderItemLiteDTO> OrderItems { get; set; }
    }
}