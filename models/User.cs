namespace shop_giay_server.models
{
    public class User: BaseEntity
    {
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public bool DeleteFlag { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}