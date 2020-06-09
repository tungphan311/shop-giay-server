namespace shop_giay_server.Dtos
{
    public class SaleProductDTO: BaseDTO
    {
        public int SaleId { get; set; }
        public SaleLiteDTO Sale { get; set; }

        public int StockId { get; set; }
        public StockLiteDTO Stock { get; set; }
    }
}