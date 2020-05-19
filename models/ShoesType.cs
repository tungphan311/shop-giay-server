using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class ShoesType: BaseEntity
    { 
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Shoes> ShoesList { get; set; }
    }
}