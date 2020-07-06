using System;
namespace shop_giay_server.models
{
    public class Provider : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TIN { get; set; } // tax identification number
    }
}