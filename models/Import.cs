using System;

namespace shop_giay_server.models
{
    public class Import
    {
        public int Id { get; set; }

        public DateTime ImportDate { get; set; }

        public int TotalQuantity { get; set; }

        public float TotalCost { get; set; }

        public int ProviderId { get; set; }
    }
}