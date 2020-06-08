namespace shop_giay_server.Dtos
{
    public class UserLiteDTO: BaseDTO
    {
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public int RoleId { get; set; }
    }

    public class UserDTO : UserLiteDTO
    {
        public RoleLiteDTO Role { get; set; }
    }
}