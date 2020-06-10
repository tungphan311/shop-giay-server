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
        private readonly RequestDelegate next;
        private readonly DataContext dataContext;
        public SignInMiddleware(RequestDelegate next, DataContext dataContext)
        {
            this.dataContext = dataContext;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authorize = context.Request.Headers["Authorization"];
            string route = context.Request.Path;

            route = route.Substring("/api/".Length);

            var isAuthorize = AuthorizePath.authorizes.FirstOrDefault(x => x == route);

            if (isAuthorize == null)
            {
                await next(context);
            }
            else
            {
                if (authorize != null && authorize.StartsWith("Bearer"))
                {
                    string token = authorize.Substring("Bearer ".Length).Trim();

                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var decodeToken = handler.ReadJwtToken(token);

                        var userId = decodeToken.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
                        var username = decodeToken.Claims.FirstOrDefault(c => c.Type == "unique_name").Value;

                        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                        if (user == null || user.Username != username)
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("User is not defined");
                            return;
                        }

                        // context.Response.StatusCode = 200;
                        // await context.Response.WriteAsync(userId);

                        await next(context);
                        // return;
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Cannot read token");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token is not defined");
                    return;
                }
            }

        }
    }
}