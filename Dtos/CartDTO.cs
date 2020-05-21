
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class CartDTO: BaseDTO
    {
        public CustomerDTO Customer { get; set; }

        public List<CartItemDTO> CartItems { get; set; }
    }
}