using System;
using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }

        public float Total { get; set; }

        public int Status { get; set; }     // 1 - Waiting for confirmation | 2 - Confirmed | 3 - Canceled

        public string DeliverAddress { get; set; }

        public DateTime? ConfirmDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string Note { get; set; }

        public string RecipientName { get; set; }

        public string RecipientPhoneNumber { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}