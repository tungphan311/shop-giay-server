using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class ShoesBrandLiteDTO : BaseDTO
    {
        public string Name { get; set; }

        public string Country { get; set; }

    }

    public class ShoesBrandDTO : ShoesBrandLiteDTO
    {
        public List<ShoesDTO> ShoesList { get; set; }
    }

}