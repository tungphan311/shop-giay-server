namespace shop_giay_server.models
{
    public class SaleProduct : BaseEntity
    {
        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        public int ShoesId { get; set; }
        public Shoes Shoes { get; set; }
    }
}