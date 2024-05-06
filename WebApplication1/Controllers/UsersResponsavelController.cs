using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserResponsavelController : ControllerBase
    {



        private readonly UserManager<IdentityUser> _userManager;

        public UserResponsavelController(
                    UserManager<IdentityUser> userManager
                )
        {
            _userManager = userManager;
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
    }

}



