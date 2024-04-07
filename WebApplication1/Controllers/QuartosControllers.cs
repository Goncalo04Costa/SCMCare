using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuartosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuartosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quarto>>> ObterTodosQuartos()
        {
            return await _context.Quartos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quarto>> ObterQuarto(int id)
        {
            var quarto = await _context.Quartos.FindAsync(id);

            if (quarto == null)
            {
                return NotFound();
            }

            return quarto;
        }

        [HttpPost]
        public async Task<ActionResult<Quarto>> InserirQuarto([FromBody] Quarto quarto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Quartos.Add(quarto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterQuarto), new { id = quarto.Id }, quarto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarQuarto(int id, [FromBody] Quarto quarto)
        {
            if (id != quarto.Id)
            {
                return BadRequest();
            }

            _context.Entry(quarto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuartoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverQuarto(int id)
        {
            var quarto = await _context.Quartos.FindAsync(id);
            if (quarto == null)
            {
                return NotFound();
            }

            _context.Quartos.Remove(quarto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuartoExists(int id)
        {
            return _context.Quartos.Any(e => e.Id == id);
        }
    }
}
