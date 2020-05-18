using System;

namespace shop_giay_server.models
{
    public class CustomerReview: BaseEntity
    {
        public DateTime Date { get; set; }

        public string Content { get; set; }

        public int Rate { get; set; }

        public int ShoesId { get; set; }
        public Shoes Shoes { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}