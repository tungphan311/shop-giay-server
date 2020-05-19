
using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Cart: BaseEntity
    {
        public Customer Customer { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}