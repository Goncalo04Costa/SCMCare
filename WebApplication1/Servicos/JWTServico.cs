using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modelos;
using WebApplication1.Modelos;

namespace WebApplication1.Servicos
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(UserFuncionario user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                CreateClaims(user),
                expires: expiration,
                signingCredentials: CreateSigningCredentials()
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Claim[] CreateClaims(UserFuncionario user) =>
     new[]
     {
        new Claim(JwtRegisteredClaimNames.Sub, user.FuncionariosId.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        new Claim("FuncionarioID", user.FuncionariosId.ToString()),
        new Claim("FuncionarioNome", user.Nome),
        new Claim("FuncionarioEmail", user.Email)
     };

        private SigningCredentials CreateSigningCredentials() =>
            new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                ),
                SecurityAlgorithms.HmacSha256
            );
    }
}
