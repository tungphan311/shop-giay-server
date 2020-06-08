using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class GenderLiteDTO: BaseDTO
    {
        public string Name { get; set; }
    }

    public class GenderDTO : GenderLiteDTO
    {
        public List<ShoesDTO> ShoesList { get; set; }

    }
}