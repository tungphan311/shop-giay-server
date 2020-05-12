namespace shop_giay_server.models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int StockId { get; set; }

        public int Amount { get; set; }

        public float PricePerUnit { get; set; }

        public float Total { get; set; }
    }
}