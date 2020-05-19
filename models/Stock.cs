namespace shop_giay_server.models
{
    public class Stock: BaseEntity
    {
        // public int Id { get; set; }
        public int Instock { get; set; }

        public int ShoesId { get; set; }
        public Shoes Shoes { get; set; }

        public int SizeId { get; set; }
        public Size Size { get; set; }

        public int ColorId { get; set; }
        public Color Color { get; set; }
    }
}