using System;
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class OrderDTO: BaseDTO 
    {
        public DateTime OrderDate { get; set; }

        public int Total { get; set; }

        public string Status { get; set; }

        public string DeliverAddress { get; set; }

        public DateTime ConfirmDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int SaleId { get; set; }
        public SaleDTO Sale { get; set; }

        public int PaymentId { get; set; }
        public PaymentDTO Payment { get; set; }

        public int CustomerId { get; set; }
        public CustomerDTO Customer { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; } 
    }
}