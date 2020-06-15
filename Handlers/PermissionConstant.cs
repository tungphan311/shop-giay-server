using System.Collections.Generic;

namespace shop_giay_server.Handlers
{
    public static class PermissionConstant
    {
        public static string Get_Shoes_Admin = "Get_Shoes_Admin";
        public static string Create_Shoes_Admin = "Create_Shoes_Admin";
        public static string Update_Shoes_Admin = "Update_Shoes_Admin";
        public static string Delete_Shoes_Admin = "Delete_Shoes_Admin";
        public static string Create_User_Admin = "Create_User_Admin";
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