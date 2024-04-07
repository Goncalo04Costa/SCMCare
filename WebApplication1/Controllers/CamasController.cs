using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CamasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cama>>> ObterTodasCamas()
        {
            var camas = await _context.Camas.ToListAsync();
            return Ok(camas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cama>> ObterCama(int id)
        {
            var cama = await _context.Camas.FindAsync(id);

            if (cama == null)
            {
                return NotFound();
            }

            return Ok(cama);
        }

        [HttpPost]
        public async Task<ActionResult<Cama>> InserirCama([FromBody] Cama cama)
        {
            if (cama == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Camas.Add(cama);
            await _context.SaveChangesAsync();

            return Ok("Cama adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCama(int id, [FromBody] Cama novaCama)
        {
            var cama = await _context.Camas.FindAsync(id);

            if (cama == null)
            {
                return NotFound($"Não foi possível encontrar a cama com o ID {id}");
            }

            cama.UtentesId = novaCama.UtentesId;
            cama.QuartosId = novaCama.QuartosId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Cama atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverCama(int id)
        {
            var cama = await _context.Camas.FindAsync(id);

            if (cama == null)
            {
                return NotFound($"Cama com o ID {id} não encontrada");
            }

            _context.Camas.Remove(cama);
            await _context.SaveChangesAsync();

            return Ok($"Cama com o ID {id} removida com sucesso");
        }
    }
}
