using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsaveisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResponsaveisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Responsavel>>> ObterTodosResponsaveis()
        {
            return await _context.Responsaveis.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Responsavel>> ObterResponsavel(int id)
        {
            var responsavel = await _context.Responsaveis.FindAsync(id);

            if (responsavel == null)
            {
                return NotFound();
            }

            return responsavel;
        }

        [HttpPost]
        public async Task<ActionResult<Responsavel>> InserirResponsavel([FromBody] Responsavel responsavel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Responsaveis.Add(responsavel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterResponsavel), new { id = responsavel.Id }, responsavel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarResponsavel(int id, [FromBody] Responsavel responsavel)
        {
            if (id != responsavel.Id)
            {
                return BadRequest();
            }

            _context.Entry(responsavel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponsavelExists(id))
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
        public async Task<IActionResult> RemoverResponsavel(int id)
        {
            var responsavel = await _context.Responsaveis.FindAsync(id);
            if (responsavel == null)
            {
                return NotFound();
            }

            _context.Responsaveis.Remove(responsavel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResponsavelExists(int id)
        {
            return _context.Responsaveis.Any(e => e.Id == id);
        }
    }
}
