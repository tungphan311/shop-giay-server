using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class SizeLiteDTO : BaseDTO
    {
        public string Name { get; set; }
    }

    public class SizeDTO : SizeLiteDTO
    {
        public List<StockLiteDTO> Stocks { get; set; }
    }
}