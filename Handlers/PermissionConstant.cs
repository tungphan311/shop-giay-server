using System.Collections.Generic;

namespace shop_giay_server.Handlers
{
    public static class PermissionConstant
    {
        public static string Create_User_Admin = "Create_User_Admin";
        public static string Get_User_Admin = "Get_User_Admin";
        public static string Update_User_Admin = "Update_User_Admin";

        public static string Get_Shoes_Admin = "Get_Shoes_Admin";
        public static string Create_Shoes_Admin = "Create_Shoes_Admin";
        public static string Update_Shoes_Admin = "Update_Shoes_Admin";
        public static string Delete_Shoes_Admin = "Delete_Shoes_Admin";

        public static string Get_ShoesBrand_Admin = "Get_ShoesBrand_Admin";
        public static string Create_ShoesBrand_Admin = "Create_ShoesBrand_Admin";
        public static string Update_ShoesBrand_Admin = "Update_ShoesBrand_Admin";
        public static string Delete_ShoesBrand_Admin = "Delete_ShoesBrand_Admin";

        public static string Get_Color_Admin = "Get_Color_Admin";
        public static string Create_Color_Admin = "Create_Color_Admin";
        public static string Update_Color_Admin = "Update_Color_Admin";
        public static string Delete_Color_Admin = "Delete_Color_Admin";

        public static string Get_Customer_Admin = "Get_Customer_Admin";
        public static string Create_Customer_Admin = "Create_Customer_Admin";
        public static string Update_Customer_Admin = "Update_Customer_Admin";
        public static string Delete_Customer_Admin = "Delete_Customer_Admin";

        public static string Get_CustomerReview_Admin = "Get_CustomerReview_Admin";
        public static string Create_CustomerReview_Admin = "Create_CustomerReview_Admin";
        public static string Update_CustomerReview_Admin = "Update_CustomerReview_Admin";
        public static string Delete_CustomerReview_Admin = "Delete_CustomerReview_Admin";

        public static string Get_CustomerType_Admin = "Get_CustomerType_Admin";
        public static string Create_CustomerType_Admin = "Create_CustomerType_Admin";
        public static string Update_CustomerType_Admin = "Update_CustomerType_Admin";
        public static string Delete_CustomerType_Admin = "Delete_CustomerType_Admin";

        public static string Get_Gender_Admin = "Get_Gender_Admin";
        public static string Create_Gender_Admin = "Create_Gender_Admin";
        public static string Update_Gender_Admin = "Update_Gender_Admin";
        public static string Delete_Gender_Admin = "Delete_Gender_Admin";

        public static string Get_Import_Admin = "Get_Import_Admin";
        public static string Create_Import_Admin = "Create_Import_Admin";
        public static string Update_Import_Admin = "Update_Import_Admin";
        public static string Delete_Import_Admin = "Delete_Import_Admin";

        public static string Get_Order_Admin = "Get_Order_Admin";
        public static string Create_Order_Admin = "Create_Order_Admin";
        public static string Update_Order_Admin = "Update_Order_Admin";
        public static string Delete_Order_Admin = "Delete_Order_Admin";

        public static string Get_Payment_Admin = "Get_Payment_Admin";
        public static string Create_Payment_Admin = "Create_Payment_Admin";
        public static string Update_Payment_Admin = "Update_Payment_Admin";
        public static string Delete_Payment_Admin = "Delete_Payment_Admin";

        public static string Get_Provider_Admin = "Get_Provider_Admin";
        public static string Create_Provider_Admin = "Create_Provider_Admin";
        public static string Update_Provider_Admin = "Update_Provider_Admin";
        public static string Delete_Provider_Admin = "Delete_Provider_Admin";

        public static string Get_Sale_Admin = "Get_Sale_Admin";
        public static string Create_Sale_Admin = "Create_Sale_Admin";
        public static string Update_Sale_Admin = "Update_Sale_Admin";
        public static string Delete_Sale_Admin = "Delete_Sale_Admin";

        public static string Get_ShoesImage_Admin = "Get_ShoesImage_Admin";
        public static string Create_ShoesImage_Admin = "Create_ShoesImage_Admin";
        public static string Update_ShoesImage_Admin = "Update_ShoesImage_Admin";
        public static string Delete_ShoesImage_Admin = "Delete_ShoesImage_Admin";

        public static string Get_ShoesType_Admin = "Get_ShoesType_Admin";
        public static string Create_ShoesType_Admin = "Create_ShoesType_Admin";
        public static string Update_ShoesType_Admin = "Update_ShoesType_Admin";
        public static string Delete_ShoesType_Admin = "Delete_ShoesType_Admin";

        public static string Get_Size_Admin = "Get_Size_Admin";
        public static string Create_Size_Admin = "Create_Size_Admin";
        public static string Update_Size_Admin = "Update_Size_Admin";
        public static string Delete_Size_Admin = "Delete_Size_Admin";

        public static string Get_Stock_Admin = "Get_Stock_Admin";
        public static string Create_Stock_Admin = "Create_Stock_Admin";
        public static string Update_Stock_Admin = "Update_Stock_Admin";
        public static string Delete_Stock_Admin = "Delete_Stock_Admin";

        public static string Get_Report_Admin = "Get_Report_Admin";
    }

    public static class PermissionPath
    {
        public static Dictionary<string, string> mapApi = new Dictionary<string, string>();
    }

    public static class AuthorizePath
    {
        public static List<string> authorizes = new List<string>()
        {
            "client/cart/get",
            "client/cart/update",
            "client/cart/add",
            "client/cart/sync",
            "client/customer/getAddresses",
            "client/customer/add-address",
            "client/customer/update-address",
            "client/order",
            "client/order/{id}",
            "client/order/list",
            "client/customer/getInfo",
            "client/customer/changePassword"
        };
    }
}