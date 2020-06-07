using System;

namespace shop_giay_server.Dtos
{
    public class CustomerReviewLiteDTO : BaseDTO
    {
        public DateTime Date { get; set; }

        public string Content { get; set; }

        public int Rate { get; set; }

        public int ShoesId { get; set; }

        public int CustomerId { get; set; }
    }

    public class CustomerReviewDTO: CustomerReviewLiteDTO
    {
        public ShoesDTO Shoes { get; set; }

        public CustomerDTO Customer { get; set; }
    }
}