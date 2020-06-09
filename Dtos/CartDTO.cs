
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class CartLiteDTO: BaseDTO
    {
        public int CustomerId { get; set; }
    }

    public class CartDTO : CartLiteDTO
    {
        public CustomerLiteDTO Customer { get; set; }

        public List<CartItemLiteDTO> CartItems { get; set; }
    }
}