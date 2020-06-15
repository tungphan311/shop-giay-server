using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using shop_giay_server.data;

namespace shop_giay_server.Handlers
{

    public class SignInMiddleware
    {
        // bug: https://stackoverflow.com/questions/48590579/cannot-resolve-scoped-service-from-root-provider-net-core-2
        private readonly RequestDelegate next;
        // private readonly DataContext dataContext;
        public SignInMiddleware(RequestDelegate next)
        {
            // this.dataContext = dataContext;
            this.next = next;
        }

        public bool needAuthorize(string route)
        {
            if (route.StartsWith("admin/"))
            {
                if (route == "admin/auth/login") return false;
                else return true;
            }
            else
            {
                var isAuthorize = AuthorizePath.authorizes.FirstOrDefault(x => x == route);

                if (isAuthorize == null) return false;
                else return true;
            }
        }

        public async Task Invoke(HttpContext context, DataContext dataContext)
        {
            string authorize = context.Request.Headers["Authorization"];
            string route = context.Request.Path;

            route = route.Substring("/api/".Length);

            var isAuthorize = needAuthorize(route);

            if (isAuthorize == false)
            {
                context.Session.SetInt32(SessionConstant.NeedAuthorize, 0);
                await next(context);
            }
            else
            {
                context.Session.SetInt32(SessionConstant.NeedAuthorize, 1);

                if (authorize != null && authorize.StartsWith("Bearer"))
                {
                    string token = authorize.Substring("Bearer ".Length).Trim();

                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var decodeToken = handler.ReadJwtToken(token);

                        var userId = decodeToken.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
                        var username = decodeToken.Claims.FirstOrDefault(c => c.Type == "unique_name").Value;
                        var role = decodeToken.Claims.FirstOrDefault(c => c.Type == "role").Value;

                        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                        if (user == null || user.Username != username)
                        {
                            await MiddlewareHelper.AccessDenied(context);
                            return;
                        }

                        context.Session.SetString(SessionConstant.Route, route);
                        context.Session.SetString(SessionConstant.Role, role);
                        context.Session.SetString(SessionConstant.Username, username);
                        await next(context);
                    }
                    else
                    {
                        await MiddlewareHelper.AccessDenied(context);
                        return;
                    }
                }
                else
                {
                    await MiddlewareHelper.AccessDenied(context);
                    return;
                }
            }

        }
    }
}