using Alumni_Back.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Alumni_Back.Repository
{
    public interface IJwtRepository
    {
        string GenerateToken(User user,DateTime expiredate,string role=null);
        Token Generate(User user,string role=null);
        JwtSecurityToken Verify(string token);


        DateTime UnixTimeStampToDateTime(long unixTimeStamp);
    }
}
