using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Account;
using WebApplication1.Modelos;

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

        public async Task<bool> AuthenticteAsync(string email, string password)
        {
            var UserF = await _context.UserFuncionario.Where(x=>x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
            if(UserF == null)
            {
                return false;
            }

            using var hmac = new HMACSHA512(UserF.PasseS);
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for(int x=0; x<=ComputedHash.Length;x++)
            {
                if (ComputedHash[x] != UserF.PasseH[x]) return false;
            }

            return true;
        }

        public string GenerateToken(int id, string email)
        {
            var claims = new[]
            {
                new Claim("id", id.ToString()),
                new Claim("email", email),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
            };

            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:secretkey"]));

            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(10);

            var token = new JwtSecurityToken(
                issuer: _configuration["jwt:issuer"],
                audience: _configuration["jwt:audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> UserExists(string email)
        {
            var UserF = await _context.UserFuncionario.Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
            if (UserF == null)
            {
                return false;
            }

            return true;
        }
    }
}
