using System;

namespace shop_giay_server.models
{
    public class Payment
    {
        public int Id { get; set; }

        public int PaymentType { get; set; }

        public string TransactionId { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public int Amount { get; set; }
    }
}