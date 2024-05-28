using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserFController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserR/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilizadorR>> GetUserFuncionario(int id)
        {
            var userR = await _context.utilizadorF.FirstOrDefaultAsync(f => f.Id == id);

            if (userR == null)
            {
                return NotFound($"User com o ID {id} não encontrado");
            }

            return Ok(userR);
        }

        // POST: api/UserR
        [HttpPost]
        public async Task<ActionResult<UtilizadorF>> PostUserFuncionario([FromBody] UtilizadorF utilizador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.utilizadorF.Add(utilizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserFuncionario), new { id = utilizador.Id }, utilizador);
        }

        // PUT: api/UserR/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserFuncionario(int id, [FromBody] UtilizadorF utilizador)
        {
            if (id != utilizador.Id)
            {
                return BadRequest("O ID do user não corresponde ao ID fornecido na solicitação");
            }

            var userF = await _context.utilizadorF.FindAsync(id);

            if (userF == null)
            {
                return NotFound($"User com o ID {id} não encontrado");
            }

            userF.UserName = utilizador.UserName;
            userF.Password = utilizador.Password;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound($"User com o ID {id} não encontrado");
                }
                else
                {
                    throw;
                }
            }

            return Ok($"User com o ID {id} atualizado com sucesso");
        }

        // DELETE: api/UserR/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserFuncionario(int id)
        {
            var userF = await _context.utilizadorF.FindAsync(id);

            if (userF == null)
            {
                return NotFound($"User com o ID {id} não encontrado");
            }

            _context.utilizadorF.Remove(userF);
            await _context.SaveChangesAsync();

            return Ok($"User com o ID {id} removido com sucesso");
        }

        private bool UserExists(int id)
        {
            return _context.utilizadorF.Any(e => e.Id == id);
        }

        internal async Task PostAsync(string v, StringContent stringContent)
        {
            throw new NotImplementedException();
        }
    }
}
