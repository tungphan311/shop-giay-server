namespace shop_giay_server.Dtos
{
    public class ShoesImageLiteDTO : BaseDTO
    {
        public int ColorId { get; set; }

        public string ImagePath { get; set; }

        public int ShoesId { get; set; }
    }

    public class ShoesImageDTO: ShoesImageLiteDTO
    {
        public ShoesDTO Shoes { get; set; }
    }
}