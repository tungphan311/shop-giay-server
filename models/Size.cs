using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Size
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Stock> Stocks { get; set; }
    }
}