using System;
using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class CustomerDTO: BaseDTO
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

        // public int CartId { get; set; }
        public CartDTO Cart { get; set; }

        public int CustomerTypeId { get; set; }
        public CustomerTypeDTO CustomerType { get; set; }

        public List<AddressDTO> Addresses { get; set; }
        public List<CustomerReviewDTO> CustomerReviews { get; set; }
        public List<OrderDTO> Orders { get; set; }
    }
}