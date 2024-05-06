using Microsoft.AspNetCore.Identity;
using WebApplication1.Modelos;

namespace WebApplication1
{
    public interface IJwtService
    {
        AuthenticationResponse CreateToken(IdentityUser user);
    }
}
