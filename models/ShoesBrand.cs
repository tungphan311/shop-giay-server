using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class ShoesBrand: BaseEntity
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public List<Shoes> ShoesList { get; set; }
    }
}