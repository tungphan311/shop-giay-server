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
            PermissionConstant.Create_Shoes, PermissionConstant.Shoes, PermissionConstant.Update_Shoes, PermissionConstant.Delete_Shoes
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