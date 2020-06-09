namespace shop_giay_server.Dtos
{
    public class ImportDetailLiteDTO: BaseDTO
    {
        public int Quantity { get; set; }

        public int OriginalPrice { get; set; }

        public int StockId { get; set; }

        public int ImportId { get; set; }

    }

    public class ImportDetailDTO : ImportDetailLiteDTO
    {
        public StockLiteDTO Stock { get; set; }

        public ImportLiteDTO Import { get; set; }
    }
}