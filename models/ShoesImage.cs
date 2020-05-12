namespace shop_giay_server.models
{
    public class ShoesImage
    {
        public int Id { get; set; }

        public int ShoesId { get; set; }

        public int ColorId { get; set; }

        public string ImagePath { get; set; }

    }
}