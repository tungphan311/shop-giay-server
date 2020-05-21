using System;

namespace shop_giay_server.Dtos
{
    public class PaymentDTO: BaseDTO
    {
        public int PaymentType { get; set; }

        public string TransactionId { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public int Amount { get; set; }

        public OrderDTO Order { get; set; }
    }
}