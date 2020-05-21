using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class GenderDTO: BaseDTO
    {
        public string Name { get; set; }

        public List<ShoesDTO> ShoesList { get; set; }
    }
}