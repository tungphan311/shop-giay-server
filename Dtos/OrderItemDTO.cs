namespace shop_giay_server.Dtos
{
    public class OrderItemDTO: BaseDTO
    {
        public int Amount { get; set; }

        public float PricePerUnit { get; set; }

        public float Total { get; set; }

        public int OrderId { get; set; }
        public OrderDTO Order { get; set; }

        public int StockId { get; set; }
        public StockDTO Stock { get; set; }
    }
}