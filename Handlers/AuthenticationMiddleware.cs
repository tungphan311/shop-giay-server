using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace shop_giay_server.Handlers
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;
        public AuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var isAuthorize = context.Session.GetInt32("NeedAuthorize");

            if (isAuthorize == 0)
            {
                await next(context);
            }
            else
            {
                string method = context.Request.Method;
                var route = context.Session.GetString("route") + "/" + method;
                var role = context.Session.GetString("role");

                var permission = PermissionPath.mapApi.FirstOrDefault(x => x.Value == route).Key;

                if (role == "admin")
                {
                    var allowed = RolePermission.admin.FirstOrDefault(x => x == permission);

                    if (allowed == null)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("You don't have permission to complete this action");
                        return;
                    }
                    else
                    {
                        await next(context);
                    }
                }
                else if (role == "staff")
                {
                    var allowed = RolePermission.staff.FirstOrDefault(x => x == permission);

                    if (allowed == null)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("You don't have permission to complete this action");
                        return;
                    }
                    else
                    {
                        await next(context);
                    }
                }
            }
        }
    }
}