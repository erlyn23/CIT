using CIT.DataAccess.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Tools
{
    public class TokenCreator
    {
        private readonly IConfiguration _configuration;

        public TokenCreator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string BuildToken(User user)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["SecretKey"];

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("Role", user.Userrole.RoleId.ToString())
            };

            var jwtSecurityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtSecurityTokenHandler.CreateToken(jwtSecurityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
