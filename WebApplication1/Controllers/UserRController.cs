using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserRController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserR/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilizadorR>> GetUserResponsavel(int id)
        {
            var userR = await _context.utilizadorR.FirstOrDefaultAsync(f => f.Id == id);

            if (userR == null)
            {
                return NotFound($"User com o ID {id} não encontrado");
            }

            return Ok(userR);
        }

        // POST: api/UserR
        [HttpPost]
        public async Task<ActionResult<UtilizadorR>> PostUserR([FromBody] UtilizadorR utilizador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.utilizadorR.Add(utilizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserResponsavel), new { id = utilizador.Id }, utilizador);
        }

        // PUT: api/UserR/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserResponsavel(int id, [FromBody] UtilizadorR utilizador)
        {
            if (id != utilizador.Id)
            {
                return BadRequest("O ID do usuário não corresponde ao ID fornecido na solicitação");
            }

            var userR = await _context.utilizadorR.FindAsync(id);

            if (userR == null)
            {
                return NotFound($"User com o ID {id} não encontrado");
            }

            userR.UserName = utilizador.UserName;
            userR.Password = utilizador.Password;

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
        public async Task<IActionResult> DeleteUserResponsavel(int id)
        {
            var userR = await _context.utilizadorR.FindAsync(id);

            if (userR == null)
            {
                return NotFound($"User com o ID {id} não encontrado");
            }

            _context.utilizadorR.Remove(userR);
            await _context.SaveChangesAsync();

            return Ok($"User com o ID {id} removido com sucesso");
        }

        private bool UserExists(int id)
        {
            return _context.utilizadorR.Any(e => e.Id == id);
        }
    }
}
