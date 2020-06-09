using System;
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class CustomerLiteDTO: BaseDTO
    {
        public string Username { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int Gender { get; set; }

        public float Point { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public int CustomerTypeId { get; set; }
    }

    public class CustomerDTO : CustomerLiteDTO
    {
        public CartLiteDTO Cart { get; set; }

        public CustomerTypeLiteDTO CustomerType { get; set; }

        public List<AddressLiteDTO> Addresses { get; set; }
        public List<CustomerReviewLiteDTO> CustomerReviews { get; set; }
        public List<OrderLiteDTO> Orders { get; set; }
    }
}