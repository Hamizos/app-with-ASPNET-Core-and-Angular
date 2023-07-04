using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;

        public TokenService(IConfiguration config)
        {
            this.config = config;
        }
        public string CreateToken(AppUser user)
        {
            var claims = new[]
                    {
                      new Claim(JwtRegisteredClaimNames.Sub,this.config["Jwt:Subject"])  ,
                      new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                      new Claim  (JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                      new Claim  ("id", user.Id.ToString()),
                      new Claim  ("UserName", user.Username),
                    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(this.config["Jwt:Issuer"], this.config["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
