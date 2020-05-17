using System;
using System.Collections.Generic;


namespace shop_giay_server.models
{
    public class Import: BaseEntity
    {
        // public int Id { get; set; }

        public DateTime ImportDate { get; set; }

        public int TotalQuantity { get; set; }

        public float TotalCost { get; set; }

        public int ProviderId { get; set; }

        // public List<ImportDetail> ImportDetails { get; set; }
    }
}