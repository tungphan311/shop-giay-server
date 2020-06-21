namespace shop_giay_server.models
{
    public class Address: LiteAddress
    {
        public Customer Customer { get; set; }
    }

    public class LiteAddress: BaseEntity
    {
        public string City { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        public string Street { get; set; }

        public int CustomerId { get; set; }
    }
}