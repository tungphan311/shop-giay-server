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
                    Seed.SeedAll(context);
                    AddPermissionPath();
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
            PermissionPath.mapApi.Add(PermissionConstant.Get_Shoes_Admin, "GET/admin/shoes");
            PermissionPath.mapApi.Add(PermissionConstant.Create_Shoes_Admin, "POST/admin/shoes");
            PermissionPath.mapApi.Add(PermissionConstant.Delete_Shoes_Admin, "DELETE/admin/shoes");
            PermissionPath.mapApi.Add(PermissionConstant.Update_Shoes_Admin, "PUT/admin/shoes");
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
