namespace shop_giay_server.Dtos
{
    public class CartItemLiteDTO: BaseDTO
    {
        public int Amount { get; set; }

        public int CartId { get; set; }
        
        public int StockId { get; set; }
    }

    public class CartItemDTO : CartItemLiteDTO
    {
        public CartLiteDTO Cart { get; set; }

        public StockLiteDTO Stock { get; set; }
    }
}