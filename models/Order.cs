using System;
using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }

        public int Total { get; set; }

        public int Status { get; set; }

        public string DeliverAddress { get; set; }

        public DateTime ConfirmDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string Note { get; set; }

        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}