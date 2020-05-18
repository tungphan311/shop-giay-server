using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Gender: BaseEntity
    {
        public string Name { get; set; }

        public List<Shoes> ShoesList { get; set; }
    }
}