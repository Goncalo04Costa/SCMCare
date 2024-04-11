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
    public class SessoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SessoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sessao>>> ObterSessoes()
        {
            return await _context.Sessoes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sessao>> ObterSessao(int id)
        {
            var sessao = await _context.Sessoes.FindAsync(id);

            if (sessao == null)
            {
                return NotFound();
            }

            return sessao;
        }

        [HttpPost]
        public async Task<ActionResult<Sessao>> InserirSessao([FromBody] Sessao sessao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Sessoes.Add(sessao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterSessao), new { id = sessao.SessaoId }, sessao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarSesao(int id, [FromBody] Sessao sessao)
        {
            if (id != sessao.SessaoId)
            {
                return BadRequest();
            }

            _context.Entry(sessao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessaoExists(id))
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
        public async Task<IActionResult> RemoverSessao(int id)
        {
            var sessao = await _context.Sessoes.FindAsync(id);
            if (sessao == null)
            {
                return NotFound();
            }

            _context.Sessoes.Remove(sessao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessaoExists(int id)
        {
            return _context.Sessoes.Any(e => e.SessaoId == id);
        }
    }
}
