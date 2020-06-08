using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class ColorLiteDTO : BaseDTO
    {
        public string Name { get; set; }
    }

    public class ColorDTO: ColorLiteDTO
    {
        public List<StockDTO> Stocks { get; set; }
    }
}