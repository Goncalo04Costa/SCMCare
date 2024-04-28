using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Servicos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDTO loginDTO)
        {
            var token = await _authService.LoginAsync(loginDTO.UserName, loginDTO.Password);
            if (token == null)
            {
                return Unauthorized("Credenciais inválidas");
            }

            return Ok(token);
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
           
            return Ok("Logout bem-sucedido");
        }



        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegistrationDTO registrationDTO)
        {
            // Validate the registration data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Attempt to register the user using AuthService
                var newUser = await _authService.RegisterAsync(registrationDTO);
                return Ok(new { message = "User registered successfully", user = newUser });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while registering the user", error = ex.Message });
            }
        }

    }
}
