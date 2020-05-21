namespace shop_giay_server.Dtos
{
    public class CartItemDTO: BaseDTO
    {
        public int Amount { get; set; }

        public int CartId { get; set; }
        public CartDTO Cart { get; set; }

        public int StockId { get; set; }
        public StockDTO Stock { get; set; }
    }
}