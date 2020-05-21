namespace shop_giay_server.Dtos
{
    public class ShoesImageDTO: BaseDTO
    {
        public int ColorId { get; set; }

        public string ImagePath { get; set; }

        public int ShoesId { get; set; }
        public ShoesDTO Shoes { get; set; }
    }
}