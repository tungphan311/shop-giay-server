using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Role: BaseEntity
    {
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}