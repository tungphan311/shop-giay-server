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
            string route = context.Request.Path;

            route = route.Substring("/api/".Length);

            var isAuthorize = AuthorizePath.authorizes.FirstOrDefault(x => x == route);

            if (isAuthorize == null)
            {
                await next(context);
            }
            else
            {
                string authorize = context.Request.Headers["Authorization"];
                string token = authorize.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                var decodeToken = handler.ReadJwtToken(token);
                var role = decodeToken.Claims.FirstOrDefault(c => c.Type == "role").Value;


                string method = context.Request.Method;

                route += "/" + method;

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