using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using my_mini_project.IServices;
using my_mini_project.ViewModel;

namespace my_mini_project.Services
{
    public class AccountService : IAccountServices
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<UserViewModel> _User;
        public AccountService(JwtSettings jwtSettings, IMongoDatabase db)
        {
            _db = db;
            _User = _db.GetCollection<UserViewModel>("users");
            _jwtSettings = jwtSettings;
        }

        public async Task<string> GenerateToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "user")
            };

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddHours(_jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Claim[]> getCaim(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "user")
            };
            return claims;
        }

        public async Task<bool> Login(string username, string password)
        {
            var filter = Builders<UserViewModel>.Filter.Eq(b => b.username, username);
            var user = await _User.Find(filter).FirstOrDefaultAsync();
            if (user == null || user.password != password)
            {
                return false;
            }
            return true;
        }
    }

}