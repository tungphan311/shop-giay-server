using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class ColorDTO: BaseDTO
    {
        public string Name { get; set; }

        public List<StockDTO> Stocks { get; set; }
    }
}