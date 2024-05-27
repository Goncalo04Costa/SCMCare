using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using System.Threading.Tasks;
using WebApplication1.Servicos;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtService _jwtService;

        public TokenController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user= await _userManager.FindByIdAsync(model.UserName);
            return Ok(new { user });
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Passe))
            {
                var token = _jwtService.GenerateToken(user.Id, "User");
                return Ok(new { token });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new Users
            {
                UserName = model.UserName,
                ID = model.ID,
                IDFuncionario = model.IDFuncionario,
                IDResponsavel = model.IDResponsavel,
                Passe = model.Passe,
            };

            var result = await _userManager.CreateAsync(user, model.Passe);

          
            return BadRequest(result.Errors);
        }
    }
}
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Passe { get; set; }
    }

    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Passe { get; set; }

        public int ID { get; set; }
        public int IDFuncionario { get; set; }
        public int IDResponsavel { get; set; }
    }
