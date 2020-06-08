using System.Collections.Generic;
using System.Runtime.Serialization;

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
        public ShoesTypeLiteDTO ShoesType { get; set; }

        public int BrandId { get; set; }
        public ShoesBrandLiteDTO ShoesBrand { get; set; }

        public int GenderId { get; set; }
        public GenderLiteDTO Gender { get; set; }

        public List<StockLiteDTO> Stocks { get; set; }
        public List<ShoesImageLiteDTO> ShoesImages { get; set; }
        public List<CustomerReviewLiteDTO> CustomerReviews { get; set; }
    }
}