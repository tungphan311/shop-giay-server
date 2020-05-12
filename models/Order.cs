using System;

namespace shop_giay_server.models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public int Total { get; set; }

        public int SaleId { get; set; }

        public string Status { get; set; }

        public int PaymentId { get; set; }

        public string DeliverAddress { get; set; }

        public DateTime ConfirmDate { get; set; }

        public DateTime DeliveryDate { get; set; }
    }
}