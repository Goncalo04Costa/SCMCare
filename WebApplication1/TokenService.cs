using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    private readonly string _secretKey = "0f1ab83a576c30f57aa5c33de4009cc923923ac041f6f63af8daa1a5ad53254a";

    public string GenerateToken(string userId, string role, string kid)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        DateTime dataCriacao = DateTime.Now;

        var header = new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));
        header["kid"] = kid;

        var payload = new JwtPayload
        {
            { "id", userId },
            { "role", role },
            { "exp", new DateTimeOffset(dataCriacao.AddHours(1.00)).ToUnixTimeSeconds() }
        };

        var token = new JwtSecurityToken(header, payload);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
