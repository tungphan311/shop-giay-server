using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using shop_giay_server.data;
using shop_giay_server.Handlers;

namespace shop_giay_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<DataContext>();

                    context.Database.Migrate();

                    // Seed.SeedShoes(context);
                    AddPermissionPath();
                    Seed.SeedAll(context);

                }
                catch (System.Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occured during migration");
                }
            }

            host.Run();
        }

        public static void AddPermissionPath()
        {
            PermissionPath.mapApi.Add(PermissionConstant.Get_Shoes_Admin, "GET/admin/shoes/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Shoes_Admin, "POST/admin/shoes/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Shoes_Admin, "DELETE/admin/shoes/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Shoes_Admin, "PUT/admin/shoes/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_ShoesBrand_Admin, "GET/admin/shoesbrand/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_ShoesBrand_Admin, "POST/admin/shoesbrand/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_ShoesBrand_Admin, "DELETE/admin/shoesbrand/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_ShoesBrand_Admin, "PUT/admin/shoesbrand/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_ShoesImage_Admin, "GET/admin/shoesimage/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_ShoesImage_Admin, "POST/admin/shoesimage/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_ShoesImage_Admin, "DELETE/admin/shoesimage/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_ShoesImage_Admin, "PUT/admin/shoesimage/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_ShoesType_Admin, "GET/admin/shoestype/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_ShoesType_Admin, "POST/admin/shoestype/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_ShoesType_Admin, "DELETE/admin/shoestype/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_ShoesType_Admin, "PUT/admin/shoestype/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Color_Admin, "GET/admin/color/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Color_Admin, "POST/admin/color/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Color_Admin, "DELETE/admin/color/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Color_Admin, "PUT/admin/color/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Customer_Admin, "GET/admin/customer/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Customer_Admin, "POST/admin/customer/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Customer_Admin, "DELETE/admin/customer/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Customer_Admin, "PUT/admin/customer/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_CustomerReview_Admin, "GET/admin/customerreview/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_CustomerReview_Admin, "POST/admin/customerreview/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_CustomerReview_Admin, "DELETE/admin/customerreview/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_CustomerReview_Admin, "PUT/admin/customerreview/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_CustomerType_Admin, "GET/admin/customertype/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_CustomerType_Admin, "POST/admin/customertype/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_CustomerType_Admin, "DELETE/admin/customertype/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_CustomerType_Admin, "PUT/admin/customertype/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Gender_Admin, "GET/admin/gender/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Gender_Admin, "POST/admin/gender/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Gender_Admin, "DELETE/admin/gender/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Gender_Admin, "PUT/admin/gender/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Import_Admin, "GET/admin/import/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Import_Admin, "POST/admin/import/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Import_Admin, "DELETE/admin/import/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Import_Admin, "PUT/admin/import/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Order_Admin, "GET/admin/order/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Order_Admin, "POST/admin/order/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Order_Admin, "DELETE/admin/order/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Order_Admin, "PUT/admin/order/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Payment_Admin, "GET/admin/payment/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Payment_Admin, "POST/admin/payment/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Payment_Admin, "DELETE/admin/payment/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Payment_Admin, "PUT/admin/payment/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Provider_Admin, "GET/admin/provider/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Provider_Admin, "POST/admin/provider/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Provider_Admin, "DELETE/admin/provider/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Provider_Admin, "PUT/admin/provider/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Sale_Admin, "GET/admin/sale/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Sale_Admin, "POST/admin/sale/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Sale_Admin, "DELETE/admin/sale/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Sale_Admin, "PUT/admin/sale/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Size_Admin, "GET/admin/size/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Size_Admin, "POST/admin/size/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Size_Admin, "DELETE/admin/size/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Size_Admin, "PUT/admin/size/");

            PermissionPath.mapApi.Add(PermissionConstant.Get_Stock_Admin, "GET/admin/stock/");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Stock_Admin, "POST/admin/stock/");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Stock_Admin, "DELETE/admin/stock/");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Stock_Admin, "PUT/admin/stock/");

            PermissionPath.mapApi.Add(PermissionConstant.Create_User_Admin, "POST/admin/auth/register");
            PermissionPath.mapApi.Add(PermissionConstant.Get_User_Admin, "GET/admin/auth");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
