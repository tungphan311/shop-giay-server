namespace shop_giay_server.models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int StockId { get; set; }

        public int Amount { get; set; }
    }
}