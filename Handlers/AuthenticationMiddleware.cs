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
            var isAuthorize = context.Session.GetInt32(SessionConstant.NeedAuthorize);

            if (isAuthorize == 0)
            {
                await next(context);
            }
            else
            {
                string method = context.Request.Method;
                var route = method + "/" + context.Session.GetString("route");
                var role = context.Session.GetString("role");

                var permission = PermissionPath.mapApi.FirstOrDefault(x => route.Contains(x.Value)).Key;

                if (role == "admin")
                {
                    var allowed = RolePermission.admin.FirstOrDefault(x => x == permission);

                    if (allowed == null)
                    {
                        await MiddlewareHelper.AccessDenied(context);
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
                        await MiddlewareHelper.AccessDenied(context);
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