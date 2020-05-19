namespace shop_giay_server.models
{
    public class CartItem: BaseEntity
    {
        public int Amount { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}