using System;

namespace shop_giay_server.Dtos
{
    public class CustomerReviewDTO: BaseDTO
    {
        public DateTime Date { get; set; }

        public string Content { get; set; }

        public int Rate { get; set; }

        public int ShoesId { get; set; }
        public ShoesDTO Shoes { get; set; }

        public int CustomerId { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}