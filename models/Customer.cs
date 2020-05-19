using System;
using System.Collections.Generic;

namespace shop_giay_server.models
{
    public class Customer: BaseEntity
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

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public int CustomerTypeId { get; set; }
        public CustomerType CustomerType { get; set; }

        public List<Address> Addresses { get; set; }
        public List<CustomerReview> CustomerReviews { get; set; }
        public List<Order> Orders { get; set; }
    }
}