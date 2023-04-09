using Alumni_Back.Models;
using Alumni_Back.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Alumni_Back.Services
{
    public class JwtService : IJwtRepository
    {
        private readonly IConfiguration configuration;
        public JwtService(IConfiguration configuration)
        {
            this.configuration= configuration;
            Key = this.configuration.GetValue<string>("Token");
        }

        string Key { get; set; }

        public Token Generate(User user,string role)
        {
            var accesstoken = GenerateToken(user, DateTime.Now.AddHours(2),role);
            var refreshtoken = GenerateToken(user, DateTime.Now.AddDays(1));

            return new Token
            {
                AccessToken = accesstoken,
                RefreshToken = refreshtoken,
                User = user,
                Role=role
            };
        }

        public string GenerateToken(User user, DateTime expiredate,string role=null)
        {
            var symmetric_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credential = new SigningCredentials(symmetric_key, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credential);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim("userId",user.Id.ToString())
            };
            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //var paylod = new JwtPayload(id.ToString(), audience: null, claims: claims, notBefore: null, expires: DateTime.Today.AddDays(1));
            var securitytoken = new JwtSecurityToken(
                claims: claims,
                expires: expiredate,
                signingCredentials: credential
                );
            return new JwtSecurityTokenHandler().WriteToken(securitytoken);
        }

        public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        public JwtSecurityToken Verify(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);
                return (JwtSecurityToken)validatedToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
