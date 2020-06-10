using System.Collections.Generic;

namespace shop_giay_server.Handlers
{
    public static class PermissionConstant
    {
        public const string Shoes = "Shoes";
        public const string Create_Shoes = "Create_Shoes";
        public const string Update_Shoes = "Update_Shoes";
        public const string Delete_Shoes = "Delete_Shoes";
        public const string Create_User = "Create_User";
    }

    public static class PermissionPath
    {
        public static Dictionary<string, string> mapApi = new Dictionary<string, string>();
    }

    public static class AuthorizePath
    {
        public static readonly List<string> authorizes = new List<string>() {
            "admin/shoes",
            "auth/register"
        };
    }
}