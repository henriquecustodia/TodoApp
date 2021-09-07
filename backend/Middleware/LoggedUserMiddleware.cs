using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Todo
{
    class LoggedUserMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggedUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext httpContext, 
            TodoContext context, 
            LoggedUserService loggedUserService)
        {
            string token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                var userEmail = jwtSecurityToken.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;

                if (userEmail != null)
                {
                    var user = await context.User.FirstOrDefaultAsync(x => x.Email == userEmail);

                    if (user != null)
                    {
                        loggedUserService.User = user;
                    }
                }
            }

            await _next(httpContext);
        }
    }
}