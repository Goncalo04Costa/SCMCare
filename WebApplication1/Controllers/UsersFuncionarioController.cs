using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class UserFuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserFuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFuncionario>>> ObterTodosUsersFuncionario(
            int? idMin = null, int? idMax = null,
            string nomeMin = null, string nomeMax = null)
        {
            IQueryable<UserFuncionario> query = _context.UserFuncionario;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.FuncionariosId >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.FuncionariosId <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMin) >= 0);
            }

            if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMax + "ZZZ") <= 0);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserFuncionario>> ObterUserFuncionario(int id)
        {
            var userFuncionario = await _context.UserFuncionario.FirstOrDefaultAsync(d => d.FuncionariosId == id);
            if (userFuncionario == null)
            {
                return NotFound();
            }
            return Ok(userFuncionario);
        }

        [HttpPost]
        public async Task<ActionResult<UserFuncionario>> InserirUserFuncionario([FromBody] UserFuncionario user)
        {
            if (user == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.UserFuncionario.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Funcionário adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUserFuncionario(int id, [FromBody] UserFuncionario novoUser)
        {
            var user = await _context.UserFuncionario.FirstOrDefaultAsync(d => d.FuncionariosId == id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o funcionário com o ID {id}");
            }

            user.Nome = novoUser.Nome;
            user.Email = novoUser.Email;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o funcionário com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverUserFuncionario(int id)
        {
            var user = await _context.UserFuncionario.FirstOrDefaultAsync(d => d.FuncionariosId == id);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o funcionário com o ID {id}");
            }

            _context.UserFuncionario.Remove(user);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o funcionário com o ID {id}");
        }
    }
}
