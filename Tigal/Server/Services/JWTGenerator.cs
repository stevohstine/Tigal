using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tigal.Shared.Models;

namespace Tigal.Server.Services
{
    public class JWTGenerator : IJWTGenerator
    {
        private readonly SymmetricSecurityKey _key;
        public JWTGenerator(IConfiguration Configuration)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["key"]));
        }
        public string GetToken(Users user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Phone));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddYears(1),
                Subject = new ClaimsIdentity(claims),
                Audience = "https://localhost:5001",
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(TokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
