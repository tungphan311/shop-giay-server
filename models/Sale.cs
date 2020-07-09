using System;
using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Sale : BaseEntity
    {
        public int SaleType { get; set; } // saletype = 1 là % = 2 là VND

        public int Amount { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime ExpiredDate { get; set; }

        public int Status { get; set; }

        public List<SaleProduct> SaleProducts { get; set; }
        public List<Order> Orders { get; set; }
    }
}