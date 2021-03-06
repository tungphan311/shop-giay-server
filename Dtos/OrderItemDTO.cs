using AutoMapper;

namespace shop_giay_server.Dtos
{
    public class OrderItemLiteDTO : BaseDTO
    {
        public int Amount { get; set; }

        public float PricePerUnit { get; set; }

        public int SaleId { get; set; }

        public float Total { get; set; }

        public int OrderId { get; set; }

        public int StockId { get; set; }

        [IgnoreMap]
        public int ShoesId { get; set; }

        [IgnoreMap]
        public string ShoesName { get; set; }

        [IgnoreMap]
        public string ImagePath { get; set; }
    }

    public class OrderItemDTO : OrderItemLiteDTO
    {
        public SaleLiteDTO Sale { get; set; }

        public OrderLiteDTO Order { get; set; }

        public StockLiteDTO Stock { get; set; }
    }
}