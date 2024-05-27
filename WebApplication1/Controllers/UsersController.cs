using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using WebApplication1.Dtos;
using WebApplication1.Modelos;
using WebApplication1.Servicos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly IJwtService _jwtService;

        public UsersController(
            UserManager<Users> userManager,
            IJwtService jwtService
        )
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<Users>> PostUser([FromBody] UsersDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Users
            {
                UserName = userDto.UserName,
                Passe = userDto.Passe,
                IDFuncionario = userDto.IDFuncionario,
                IDResponsavel = userDto.IDResponsavel
            };

            var result = await _userManager.CreateAsync(user, userDto.Passe);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Clear sensitive data
            user.Passe = null;

            return Created("", user);
        }

        // GET: api/Users/username
        [HttpGet("{username}")]
        public async Task<ActionResult<Users>> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            // Clear sensitive data
            user.Passe = null;

            return user;
        }

        // POST: api/Users/BearerToken
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
