using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using WebApplication1.Modelos;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public TokenController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("create")]
        public IActionResult CreateToken(int id, string role)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            // Configuração do token JWT
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", id.ToString()),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Criação e serialização do token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);


            _context.Tokens.Add(new Token { TokenValue = tokenString });
            _context.SaveChanges();

            return Ok(new { token = tokenString });
        }

        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                // Validação do token JWT
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,    // Opcional: Pode ser configurado conforme seu cenário
                    ValidateAudience = false,  // Opcional: Pode ser configurado conforme seu cenário
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return Ok(new { message = "Valid token" });
            }
            catch
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }

        // Outros métodos como deleteToken, renewToken podem ser adicionados aqui

    }
}
