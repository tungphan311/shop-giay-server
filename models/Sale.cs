using System;

namespace shop_giay_server.models
{
    public class Sale
    {
        public int Id { get; set; }

        public int SaleType { get; set; }

        public int Amount { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime ExpiredDate { get; set; }

        public string Status { get; set; }
    }
}