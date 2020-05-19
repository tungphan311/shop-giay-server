using System;
using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Sale: BaseEntity
    {
        public int SaleType { get; set; }

        public int Amount { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime ExpiredDate { get; set; }

        public string Status { get; set; }

        public List<SaleProduct> SaleProducts { get; set; }
        public List<Order> Orders{ get; set; }
    }
}