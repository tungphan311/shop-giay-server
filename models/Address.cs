namespace shop_giay_server.models
{
    public class Address
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        public string Street { get; set; }

        public int CustomerId { get; set; }
    }
}