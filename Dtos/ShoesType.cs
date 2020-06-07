using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class ShoesTypeDTO: BaseDTO
    { 
        public string Name { get; set; }

        public string Description { get; set; }

        // public List<ShoesDTO> ShoesList { get; set; }
    }
}