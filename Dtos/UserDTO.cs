namespace shop_giay_server.Dtos
{
    public class UserDTO: BaseDTO
    {
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public bool DeleteFlag { get; set; }

        public int RoleId { get; set; }
        public RoleDTO Role { get; set; }
    }
}