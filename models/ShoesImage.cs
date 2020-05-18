namespace shop_giay_server.models
{
    public class ShoesImage: BaseEntity
    {
        public int ColorId { get; set; }

        public string ImagePath { get; set; }

        public int ShoesId { get; set; }
        public Shoes Shoes { get; set; }
    }
}