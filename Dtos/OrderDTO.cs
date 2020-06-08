using System;
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class OrderLiteDTO: BaseDTO 
    {
        public DateTime OrderDate { get; set; }

        public int Total { get; set; }

        public string Status { get; set; }

        public string DeliverAddress { get; set; }

        public DateTime ConfirmDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int SaleId { get; set; }
        
        public int PaymentId { get; set; }
        
        public int CustomerId { get; set; }
    }

    public class OrderDTO : OrderLiteDTO
    {
        public SaleLiteDTO Sale { get; set; }

        public PaymentLiteDTO Payment { get; set; }

        public CustomerLiteDTO Customer { get; set; }

        public List<OrderItemLiteDTO> OrderItems { get; set; }
    }
}