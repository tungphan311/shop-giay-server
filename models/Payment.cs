using System;

namespace shop_giay_server.models
{
    public class Payment: BaseEntity
    {
        public int PaymentType { get; set; }

        public string TransactionId { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public int Amount { get; set; }

        public Order Order { get; set; }
    }
}