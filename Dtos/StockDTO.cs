namespace shop_giay_server.Dtos
{
    public class StockDTO: StockLiteDTO
    {
        public ShoesDTO Shoes { get; set; }

        public SizeDTO Size { get; set; }

        public ColorDTO Color { get; set; }
    }

    public class StockLiteDTO : BaseDTO
    {
        public int Instock { get; set; }

        public int ShoesId { get; set; }

        public int SizeId { get; set; }

        public int ColorId { get; set; }
    }
}