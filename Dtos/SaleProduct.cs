namespace shop_giay_server.Dtos
{
    public class SaleProductDTO: BaseDTO
    {
        public int SaleId { get; set; }
        public SaleDTO Sale { get; set; }

        public int StockId { get; set; }
        public StockDTO Stock { get; set; }
    }
}