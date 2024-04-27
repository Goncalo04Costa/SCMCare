using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PratosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PratosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prato>>> ObterTodosPratos(
            int? idMin = null, int? idMax = null,
            string nomeMin = null, string nomeMax = null,
            string descMin = null, string descMax = null,
            bool tipo0 = false, bool tipo1 = false,
            bool ativo0 = false, bool ativo1 = false)
        {
            IQueryable<Prato> query = _context.Pratos;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMin) >= 0);
            }

            if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMax + "ZZZ") <= 0);
            }

            if (!string.IsNullOrEmpty(descMin))
            {
                query = query.Where(d => d.Descricao.CompareTo(descMin) >= 0);
            }

            if (!string.IsNullOrEmpty(descMax))
            {
                query = query.Where(d => d.Descricao.CompareTo(descMax + "ZZZ") <= 0);
            }

            if (tipo0 && !tipo1)
            {
                query = query.Where(d => !d.Tipo); // Mostra pratos com tipo 0
            }
            else if (!tipo0 && tipo1)
            {
                query = query.Where(d => d.Tipo); // Mostra pratos com tipo 1
            }

            if (ativo0 && !ativo1)
            {
                query = query.Where(d => !d.Ativo);
            }

            else if (!ativo0 && ativo1)
            {
                query = query.Where(d => d.Ativo);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Prato>> ObterPrato(int id)
        {
            var prato = await _context.Pratos.FindAsync(id);

            if (prato == null)
            {
                return NotFound();
            }

            return prato;
        }

        [HttpPost]
        public async Task<ActionResult<Prato>> InserirPrato([FromBody] Prato prato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pratos.Add(prato);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPrato), new { id = prato.Id }, prato);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPrato(int id, [FromBody] Prato prato)
        {
            if (id != prato.Id)
            {
                return BadRequest();
            }

            _context.Entry(prato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PratoExists(id))
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
        public async Task<IActionResult> RemoverPrato(int id)
        {
            var prato = await _context.Pratos.FindAsync(id);
            if (prato == null)
            {
                return NotFound();
            }

            _context.Pratos.Remove(prato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PratoExists(int id)
        {
            return _context.Pratos.Any(e => e.Id == id);
        }
    }
}
