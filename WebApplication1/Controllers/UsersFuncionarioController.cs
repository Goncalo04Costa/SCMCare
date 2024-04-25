using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modelos;
using System.Threading.Tasks;
using WebApplication1.Modelos;
using WebApplication1.Servicos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersFuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;

        public UsersFuncionarioController(AppDbContext context, IConfiguration configuration, JwtService jwtService)
        {
            _context = context;
            _configuration = configuration;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo de login inválido");
            }

            var user = await _context.UsersFuncionario.FirstOrDefaultAsync(u => u.User == loginViewModel.Username);
            if (user == null || user.Passe != loginViewModel.Password)
            {
                return Unauthorized("Credenciais inválidas");
            }

            // Autenticação bem-sucedida, gera o token JWT
            var token = _jwtService.GenerateJwtToken(user);
            return Ok(token);
        }
    }
}
