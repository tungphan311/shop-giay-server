using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Shoes : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Rating { get; set; }

        public float Price { get; set; }

        public bool IsNew { get; set; }

        public bool IsOnSale { get; set; }

        public int StyleId { get; set; }
        public ShoesType ShoesType { get; set; }

        public int BrandId { get; set; }
        public ShoesBrand ShoesBrand { get; set; }

        public int GenderId { get; set; }
        public Gender Gender { get; set; }

        public List<Stock> Stocks { get; set; }
        public List<ShoesImage> ShoesImages { get; set; }
        public List<CustomerReview> CustomerReviews { get; set; }
    }
}