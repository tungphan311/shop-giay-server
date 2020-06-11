using System.Collections.Generic;

namespace shop_giay_server.Handlers
{
    public static class PermissionConstant
    {
        public static string Shoes = "Shoes";
        public static string Create_Shoes = "Create_Shoes";
        public static string Update_Shoes = "Update_Shoes";
        public static string Delete_Shoes = "Delete_Shoes";
        public static string Create_User = "Create_User";
    }

    public static class PermissionPath
    {
        public static Dictionary<string, string> mapApi = new Dictionary<string, string>();
    }

    public static class AuthorizePath
    {
        public static List<string> authorizes = new List<string>()
        {

        };
    }
}