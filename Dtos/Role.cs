using System.Collections.Generic;

namespace shop_giay_server.Dtos
{
    public class RoleDTO: BaseDTO
    {
        public string Name { get; set; }

        public List<UserDTO> Users { get; set; }
    }
}