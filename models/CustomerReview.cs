using System;

namespace shop_giay_server.models
{
    public class CustomerReview
    {
        public int Id { get; set; }

        public int ShoesId { get; set; }

        public int CustomerId { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }

        public int Rate { get; set; }
    }
}