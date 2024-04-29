using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication1.Modelos;
using Microsoft.Extensions.Configuration;
using WebApplication1.Account;

namespace WebApplication1.Identity
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticateService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            var user = await _context.UserFuncionario.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                return false;
            }

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(user.SecurityStamp));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Comparando os hashes de senha como strings
            if (Convert.ToBase64String(computedHash) != user.PasswordHash)
            {
                return false;
            }

            return true;
        }

        public Task<bool> AuthenticteAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public string GenerateToken(int id, string email)
        {
            var claims = new[]
            {
                new Claim("id", id.ToString()),
                new Claim("email", email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> UserExists(string email)
        {
            var user = await _context.UserFuncionario.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            return user != null;
        }
    }
}
