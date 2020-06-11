using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace shop_giay_server.Handlers
{
    public static class MiddlewareHelper
    {
        public static async Task AccessDenied(HttpContext context)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("You don't have permission to complete this action");
            return;
        }
    }
}