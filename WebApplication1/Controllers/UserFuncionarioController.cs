using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Modelos;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserFuncionarioController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserToken>> Incluir(UserFDTO userDto)
        {
          

            return CreatedAtAction(nameof(Incluir), new { id = userDto.ID }, new UserToken()); // Exemplo de retorno com código 201 (Created) e um objeto UserToken vazio
        }
    }
}
