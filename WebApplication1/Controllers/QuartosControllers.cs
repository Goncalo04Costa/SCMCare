using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


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
        public async Task<ActionResult<IEnumerable<Quarto>>> ObterTodosQuartos(
            int? idMin = null, int? idMax = null,
            int? numeroMin = null, int? numeroMax = null,
            int? tipoQuarto = null)
        {
            IQueryable<Quarto> query = _context.Quartos;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (numeroMin.HasValue)
            {
                query = query.Where(d => d.Numero >= numeroMin.Value);
            }

            if (numeroMax.HasValue)
            {
                query = query.Where(d => d.Numero <= numeroMax.Value);
            }

            if (tipoQuarto.HasValue)
            {
                query = query.Where(d => d.TiposQuartoId == tipoQuarto.Value);
            }

            var quartosDetalhes = await (
                from quartos in query
                join tipoquarto in _context.TiposQuarto on quartos.TiposQuartoId equals tipoquarto.Id into tG
                from tipoquarto in tG.DefaultIfEmpty()
                select new
                {
                    Id = quartos.Id,
                    Numero = quartos.Numero,
                    TiposQuartoId = quartos.TiposQuartoId,
                    TiposQuarto = tipoquarto.Descricao
                }
            ).ToListAsync();

            return Ok(quartosDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quarto>> ObterQuarto(int id)
        {
            IQueryable<Quarto> query = _context.Quartos;
            query = query.Where(d => d.Id == id);

            var quartosDetalhes = await (
                from quartos in query
                join tipoquarto in _context.TiposQuarto on quartos.TiposQuartoId equals tipoquarto.Id into tG
                from tipoquarto in tG.DefaultIfEmpty()
                select new
                {
                    Id = quartos.Id,
                    Numero = quartos.Numero,
                    TiposQuartoId = quartos.TiposQuartoId,
                    TiposQuarto = tipoquarto.Descricao
                }
            ).ToListAsync();

            return Ok(quartosDetalhes);
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
