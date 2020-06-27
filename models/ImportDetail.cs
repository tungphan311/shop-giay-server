namespace shop_giay_server.models
{
    public class ImportDetail : BaseEntity
    {
        public int Quantity { get; set; }

        public float OriginalPrice { get; set; }

        public int StockId { get; set; }
        public Stock Stock { get; set; }

        public int ImportId { get; set; }
        public Import Import { get; set; }

    }
}