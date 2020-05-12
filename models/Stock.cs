namespace shop_giay_server.models
{
    public class Stock
    {
        public int Id { get; set; }

        public int ShoesId { get; set; }

        public int SizeId { get; set; }

        public int ColorId { get; set; }

        public int Instock { get; set; }
    }
}