using Alumni_Back.Repository;
using Alumni_Back.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Alumni_Back
{
    public class Authorization : Attribute, IAuthorizationFilter
    {
        public string Role { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var api_key))
            {
                context.Result = new UnauthorizedObjectResult(new { Error = "You are not authentificated" });
                return;
            }

            var jwt = context.HttpContext.RequestServices.GetService<IJwtRepository>();
            var user_service = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var validated_token = jwt.Verify(api_key);
            if (validated_token == null)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Error = "Error on authentication"
                });
                return;
            }
            var expClaim = validated_token.Claims.FirstOrDefault(c => c.Type == "exp");
            DateTime expirationDate = jwt.UnixTimeStampToDateTime(long.Parse(expClaim?.Value));
            if (expirationDate < DateTime.Now)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Error = "Token expired"
                });
                return;
            }
            var username = validated_token.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            var userId = int.Parse(validated_token.Claims.First(claim => claim.Type == "userId").Value);
            var user = user_service.Get(userId);

            user.Switch(
                _ => context.Result = new UnauthorizedObjectResult(new { Error = _ }),
                u =>
                {
                    if(Role!=null && user_service.GetRole(u)!=Role)
                    {
                        context.Result = new UnauthorizedObjectResult(new { Error = "You are not authorised" });
                    }
                    else
                    {
                        user_service.ConnectedUser = u;
                    }
                });
            return;
        }
    }
}
