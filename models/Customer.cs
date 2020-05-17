using System;

namespace shop_giay_server.models
{
    public class Customer
    {
        public int Id { get; set; }

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
}