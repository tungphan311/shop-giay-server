namespace shop_giay_server.Dtos
{
    public class UserForRegisterDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}