using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersResponsavelController : Controller
    {
        private readonly AppDbContext _context;

        public UsersResponsavelController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponsavel>>> ObterTodosUsersResponsavel(
            int? idMin = null, int? idMax = null,
            string userMin = null, string userMax = null)
        {
            IQueryable<UserResponsavel> query = _context.UserResponsavel;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.ResponsaveisId >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.ResponsaveisId <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(userMin))
            {
                query = query.Where(d => d.User.CompareTo(userMin) >= 0);
            }

            if (!string.IsNullOrEmpty(userMax))
            {
                query = query.Where(d => d.User.CompareTo(userMax + "ZZZ") <= 0);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponsavel>> ObterUserResponsavel(int id)
        {
            var dado = await _context.UserResponsavel.FirstOrDefaultAsync(d => d.ResponsaveisId == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponsavel>> InserirUserResponsavel([FromBody] UserResponsavel user)
        {
            if (user == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.UserResponsavel.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Utilizador adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaUserResponsavel(int id, [FromBody] UserResponsavel novoUser)
        {
            var user = await _context.UserResponsavel.FirstOrDefaultAsync(d => d.ResponsaveisId == id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o utilizador com o ID {id}");
            }

            user.User = novoUser.User;
            user.Passe = novoUser.Passe;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o utilizador com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUserResponsavel(int id)
        {
            var user = await _context.UserResponsavel.FirstOrDefaultAsync(d => d.ResponsaveisId == id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o utilizador com o ID {id}");
            }

            _context.UserResponsavel.Remove(user);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o utilizador com o ID {id}");
        }
    }
}
