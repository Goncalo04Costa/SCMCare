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
    public class PrescricoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PrescricoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescricao>>> ObterTodasPrescricoes()
        {
            return await _context.Prescricoes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Prescricao>> ObterPrescricao(int id)
        {
            var prescricao = await _context.Prescricoes.FindAsync(id);

            if (prescricao == null)
            {
                return NotFound();
            }

            return prescricao;
        }

        [HttpPost]
        public async Task<ActionResult<Prescricao>> InserirPrescricao([FromBody] Prescricao prescricao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Prescricoes.Add(prescricao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPrescricao), new { id = prescricao.Id }, prescricao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPrescricao(int id, [FromBody] Prescricao prescricao)
        {
            if (id != prescricao.Id)
            {
                return BadRequest();
            }

            _context.Entry(prescricao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescricaoExists(id))
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
        public async Task<IActionResult> RemoverPrescricao(int id)
        {
            var prescricao = await _context.Prescricoes.FindAsync(id);
            if (prescricao == null)
            {
                return NotFound();
            }

            _context.Prescricoes.Remove(prescricao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrescricaoExists(int id)
        {
            return _context.Prescricoes.Any(e => e.Id == id);
        }
    }
}
