namespace shop_giay_server.models
{
    public class SaleProduct: BaseEntity
    {
        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}