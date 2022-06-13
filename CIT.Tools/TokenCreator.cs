using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly ILenderBusinessRepository _lenderBusinessRepository;
        private int _userId;

        public TokenCreator(IConfiguration configuration, 
            ILenderBusinessRepository lenderBusinessRepository)
        {
            _configuration = configuration;
            _lenderBusinessRepository = lenderBusinessRepository;
        }
        public string BuildToken(User user, int lenderBusinessId)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["SecretKey"];

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Role", user.Userrole.RoleId.ToString()),
                new Claim("LenderBusinessId", lenderBusinessId.ToString()),
                new Claim("UserType", "User")
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

        public string BuildToken(LenderBusiness lenderBusiness)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["SecretKey"];

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, lenderBusiness.Email),
                new Claim(ClaimTypes.NameIdentifier, lenderBusiness.Id.ToString()),
                new Claim("Role", lenderBusiness.LenderRole.RoleId.ToString()),
                new Claim("UserType", "Lender")
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

        public int GetUserId(HttpRequest request)
        {
            var userType = GetUserTypeAndUserId(request);

            if (userType.Equals("User"))
                return _userId;

            return 0;
        }

        public async Task<int> GetLenderBusinessId(HttpRequest request)
        {
            var userData = GetUserTypeAndUserId(request).Split(' ');
            int lenderBusinessId;
            if (userData[0].Equals("User"))
                int.TryParse(userData[1], out lenderBusinessId);
            else
            {
                var lenderBusiness = await _lenderBusinessRepository.FirstOrDefaultAsync(l => l.Id == _userId);
                lenderBusinessId = lenderBusiness.Id;
            }
            return lenderBusinessId;
        }

        private string GetUserTypeAndUserId(HttpRequest request)
        {
            var decodedToken = DecodeToken(request);
            _userId = int.Parse(decodedToken.Claims.Where(c => c.Type.Equals("nameid")).FirstOrDefault().Value);
            var lenderBusinessClaim = decodedToken.Claims.Where(c => c.Type.Equals("LenderBusinessId")).FirstOrDefault();

            if (lenderBusinessClaim == null)
                return $"{decodedToken.Claims.Where(c => c.Type.Equals("UserType")).FirstOrDefault().Value}";
            else
                return $"{decodedToken.Claims.Where(c => c.Type.Equals("UserType")).FirstOrDefault().Value} {lenderBusinessClaim.Value}";
        }

        public bool HasTokenExpired(HttpRequest request)
        {
            var stringToken = GetStringToken(request);
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(stringToken, new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["SecretKey"])),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return DateTime.Now == validatedToken.ValidTo;
        }

        public JwtSecurityToken DecodeToken(HttpRequest request)
        {
            var stringToken = GetStringToken(request);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(stringToken);
            return jsonToken;
        }

        private string GetStringToken(HttpRequest request) 
        {
            var bearerHeader = request.Headers["Authorization"];
            var stringToken = bearerHeader.ToString().Replace("Bearer ", "");
            return stringToken;
        }
    }
}
