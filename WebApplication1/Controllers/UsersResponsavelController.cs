using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using WebApplication1.Modelos;
using WebApplication1.Servicos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserResponsavelController : ControllerBase
    {



        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;

        public UserResponsavelController(
                    UserManager<IdentityUser> userManager,
                    JwtService jwtService
                )
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }


        [HttpPost]
        public async Task<ActionResult<UserResponsavel>> PostUser(UserResponsavel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateAsync(
                new IdentityUser() { UserName = user.User },
                user.Passe
            );

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            user.Passe = null;
            return Created("", user);
        }


        // GET: api/Users/username
        [HttpGet("{username}")]
        public async Task<ActionResult<UserResponsavel>> GetUser(string username)
        {
            IdentityUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return new UserResponsavel
            {
                User = user.UserName,
            };
        }

        [HttpPost("BearerToken")]
        public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad credentials");
            }

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return BadRequest("Bad credentials");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var token = _jwtService.CreateToken(user);

            return Ok(token);
        }

    }
}



