using Microsoft.AspNetCore.Identity;
using WebApplication1.Modelos;

namespace WebApplication1.Servicos
{
    public interface IJwtService
    {
        AuthenticationResponse CreateToken(IdentityUser user);
    }
}
