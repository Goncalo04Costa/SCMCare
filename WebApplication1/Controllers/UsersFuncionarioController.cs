using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersFuncionarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsersFuncionarioController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFuncionario>>> ObterTodosUsersFuncionario(
            int? idMin = null, int? idMax = null,
            string userMin = null, string userMax = null)
        {
            IQueryable<UserFuncionario> query = _context.UsersFuncionario;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.FuncionariosId >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.FuncionariosId <= idMax.Value);
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
        public async Task<ActionResult<UserFuncionario>> ObterUserFuncionario(int id)
        {
            var dado = await _context.UsersFuncionario.FirstOrDefaultAsync(d => d.FuncionariosId == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<UserFuncionario>> InserirUserFuncionario([FromBody] UserFuncionario user)
        {
            if (user == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.UsersFuncionario.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Utilizador adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaUserFuncionario(int id, [FromBody] UserFuncionario novoUser)
        {
            var user = await _context.UsersFuncionario.FirstOrDefaultAsync(d => d.FuncionariosId == id);
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
        public async Task<IActionResult> RemoveUserFuncionario(int id)
        {
            var user = await _context.UsersFuncionario.FirstOrDefaultAsync(d => d.FuncionariosId == id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o utilizador com o ID {id}");
            }

            _context.UsersFuncionario.Remove(user);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o utilizador com o ID {id}");
        }
    }
}
