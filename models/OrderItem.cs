using System.ComponentModel.DataAnnotations.Schema;

namespace shop_giay_server.models
{
    public class OrderItem : BaseEntity
    {
        public int Amount { get; set; }

        public float PricePerUnit { get; set; }

        public int? SaleId { get; set; }

        [ForeignKey("SaleId")]
        public virtual Sale Sale { get; set; }

        public float Total { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}