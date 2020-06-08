using System;
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class SaleLiteDTO: BaseDTO
    {
        public int SaleType { get; set; }

        public int Amount { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime ExpiredDate { get; set; }

        public string Status { get; set; }
    }

    public class SaleDTO : SaleLiteDTO
    {
        public List<SaleProductDTO> SaleProducts { get; set; }
        public List<OrderDTO> Orders { get; set; }
    }
}