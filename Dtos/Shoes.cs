using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class ShoesDTO: BaseDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Rating { get; set; }

        public float Price { get; set; }

        public bool IsNew { get; set; }

        public bool IsOnSale { get; set; }

        public int StyleId { get; set; }
        public ShoesTypeDTO ShoesType { get; set; }

        public int BrandId { get; set; }
        public ShoesBrandDTO ShoesBrand { get; set; }

        public int GenderId { get; set; }
        public GenderDTO Gender { get; set; }

        public List<StockDTO> Stocks { get; set; }
        public List<ShoesImageDTO> ShoesImages { get; set; }
        public List<CustomerReviewDTO> CustomerReviews { get; set; }
    }
}