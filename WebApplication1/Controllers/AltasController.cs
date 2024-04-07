using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AltasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AltasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alta>>> ObterTodasAltas()
        {
            var altas = await _context.Alta.ToListAsync();
            return Ok(altas);
        }

        [HttpGet("{utenteId}/{funcionarioId}")]
        public async Task<ActionResult<Alta>> ObterAlta(int utenteId, int funcionarioId)
        {
            var alta = await _context.Alta
                .FirstOrDefaultAsync(a => a.UtentesId == utenteId && a.FuncionariosId == funcionarioId);

            if (alta == null)
            {
                return NotFound();
            }

            return Ok(alta);
        }

        [HttpPost]
        public async Task<ActionResult<Alta>> InserirAlta([FromBody] Alta alta)
        {
            if (alta == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Alta.Add(alta);
            await _context.SaveChangesAsync();

            return Ok("Alta adicionada com sucesso");
        }

        [HttpPut("{utenteId}/{funcionarioId}")]
        public async Task<IActionResult> AtualizarAlta(int utenteId, int funcionarioId, [FromBody] Alta novaAlta)
        {
            var alta = await _context.Alta
                .FirstOrDefaultAsync(a => a.UtentesId == utenteId && a.FuncionariosId == funcionarioId);

            if (alta == null)
            {
                return NotFound($"Não foi possível encontrar a alta para o utente ID {utenteId} e funcionário ID {funcionarioId}");
            }

            alta.Data = novaAlta.Data;
            alta.Motivo = novaAlta.Motivo;
            alta.Destino = novaAlta.Destino;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Alta atualizada para o utente ID {utenteId} e funcionário ID {funcionarioId}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{utenteId}/{funcionarioId}")]
        public async Task<IActionResult> RemoverAlta(int utenteId, int funcionarioId)
        {
            var alta = await _context.Alta
                .FirstOrDefaultAsync(a => a.UtentesId == utenteId && a.FuncionariosId == funcionarioId);

            if (alta == null)
            {
                return NotFound($"Não foi possível encontrar a alta para o utente ID {utenteId} e funcionário ID {funcionarioId}");
            }

            _context.Alta.Remove(alta);
            await _context.SaveChangesAsync();

            return Ok($"Alta removida para o utente ID {utenteId} e funcionário ID {funcionarioId}");
        }
    }
}
