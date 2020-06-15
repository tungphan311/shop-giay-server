using shop_giay_server.Dtos;
using AutoMapper;
using System.Collections.Generic;

namespace shop_giay_server._Controllers 
{
    public class BodyCreateShoes
    {
        public bool DeleteFlag { get; set; } = false;
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; } = 0;
        public float Price { get; set; }
        public bool IsNew { get; set; } = false;
        public bool IsOnSale { get; set; } = false;
        public int StyleId { get; set; }
        public int BrandId { get; set; }
        public int GenderId { get; set; }
        public List<ShoesImageDTO> Images { get; set; } = new List<ShoesImageDTO>();
        public List<StockDTO> Stocks { get; set; } = new List<StockDTO>();

        public bool IsValid()
        {
            return Code != null
                && Name != null
                && Price > 0
                && StyleId > 0
                && BrandId > 0
                && GenderId > 0;
        }
    }

    public class BodyUpdateShoesStock
    {
        public int ShoesId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
    }

    public class ResponseShoesDTO : BaseDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public int isNew { get; set; }
        public int isOnSale { get; set; }
        public string imagePath { get; set; }
        public int quantity { get; set; }
        public int salePrice { get; set; }
        public string styleName { get; set; }
    }

    public class ResponseShoesDetailDTO : BaseDTO
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float rating { get; set; }
        public float ratingCount { get; set; }
        public string styleName { get; set; }
        public string brandName { get; set; }
        public string genderName { get; set; }
        public int price { get; set; }
        public int isNew { get; set; }
        public int isOnSale { get; set; }
        public int salePrice { get; set; }
        public List<string> images { get; set; } = new List<string>();

        [IgnoreMap]
        public List<dynamic> sizes { get; set; } = new List<dynamic>();
    }

    public class BodyShoesRating 
    {
        public int shoesId { get; set; }
        public int rating { get; set; }
    }
}
    