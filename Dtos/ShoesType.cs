using System.Collections.Generic;

namespace shop_giay_server.Dtos
{

    public class ShoesTypeLiteDTO : BaseDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

    }

    public class ShoesTypeDTO: ShoesTypeLiteDTO
    { 
        public List<ShoesLiteDTO> ShoesList { get; set; }
    }
}