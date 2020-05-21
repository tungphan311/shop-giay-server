namespace shop_giay_server.Dtos
{
    public class AddressDTO: BaseDTO
    {
        public string City { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        public string Street { get; set; }

        public int CustomerId { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}