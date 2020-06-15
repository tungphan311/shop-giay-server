using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace shop_giay_server.Handlers
{
    public static class RolePermission
    {
        public static List<string> admin = getAllPermissions();
        public static List<string> staff = new List<string>() {
            PermissionConstant.Get_Shoes_Admin, PermissionConstant.Create_Shoes_Admin, PermissionConstant.Update_Shoes_Admin, PermissionConstant.Delete_Shoes_Admin
        };

        public static List<string> getAllPermissions()
        {
            List<string> permissions = new List<string>();

            Type t = typeof(PermissionConstant);
            FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                permissions.Add(field.GetValue(null).ToString());
            }

            return permissions;
        }
    }
}