namespace shop_giay_server.models
{
    public class ImportDetail
    {
        public int Id { get; set; }

        public int ImportId { get; set; }

        public int Quantity { get; set; }

        public int OriginalPrice { get; set; }

        public int StockId { get; set; }
    }
}