namespace shop_giay_server.models
{
    public class Shoes
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Rating { get; set; }

        public int StyleId { get; set; }

        public int BrandId { get; set; }

        public int GenderId { get; set; }

        public float Price { get; set; }

        public bool IsNew { get; set; }

        public bool IsOnSale { get; set; }
    }
}