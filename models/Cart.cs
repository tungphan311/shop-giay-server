
using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Cart : BaseEntity
    {
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}