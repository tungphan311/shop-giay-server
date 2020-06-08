using System;

namespace shop_giay_server.Dtos
{
    public class PaymentLiteDTO: BaseDTO
    {
        public int PaymentType { get; set; }

        public string TransactionId { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public int Amount { get; set; }
    }

    public class PaymentDTO : PaymentLiteDTO
    {
        public OrderLiteDTO Order { get; set; }
    }
}