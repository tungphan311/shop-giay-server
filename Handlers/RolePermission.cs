using System.Collections.Generic;
using System.Collections.Immutable;

namespace shop_giay_server.Handlers
{
    public static class RolePermission
    {
        public static readonly List<string> admin = new List<string>() { PermissionConstant.Create_User, PermissionConstant.Shoes };
        public static readonly List<string> staff = new List<string>() { };
    }
}