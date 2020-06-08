using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class RoleLiteDTO: BaseDTO
    {
        public string Name { get; set; }
    }

    public class RoleDTO : RoleLiteDTO
    {
        public List<UserLiteDTO> Users { get; set; }
    }
}