namespace shop_giay_server.Dtos
{
    public class ImportDetailDTO: BaseDTO
    {
        public int Quantity { get; set; }

        public int OriginalPrice { get; set; }

        public int StockId { get; set; }
        public StockDTO Stock { get; set; }

        public int ImportId { get; set; }
        public ImportDTO Import { get; set; }

    }
}