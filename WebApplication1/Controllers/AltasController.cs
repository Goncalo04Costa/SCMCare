using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<ActionResult<IEnumerable<Alta>>> ObterTodasAltas(
            int? utentesIdMin = null, int? utentesIdMax = null,
            int? funcionariosIdMin = null, int? funcionariosIdMax = null,
            DateTime? dataMin = null, DateTime? dataMax = null)
        {
            IQueryable<Alta> query = _context.Alta;

            if (utentesIdMin.HasValue)
            {
                query = query.Where(d => d.UtentesId >= utentesIdMin.Value);
            }

            if (utentesIdMax.HasValue)
            {
                query = query.Where(d => d.UtentesId <= utentesIdMax.Value);
            }

            if (funcionariosIdMin.HasValue)
            {
                query = query.Where(d => d.FuncionariosId >= funcionariosIdMin.Value);
            }

            if (funcionariosIdMax.HasValue)
            {
                query = query.Where(d => d.FuncionariosId <= funcionariosIdMax.Value);
            }

            if (dataMin.HasValue)
            {
                query = query.Where(d => d.Data >= dataMin.Value);
            }

            if (dataMax.HasValue)
            {
                query = query.Where(d => d.Data <= dataMax.Value);
            }


            var altasDetalhes = await (
                from alta in query
                join utente in _context.Utentes on alta.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on alta.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                select new
                {
                    UtentesId = alta.UtentesId,
                    Utentes = utente.Nome,
                    FuncionarioId = alta.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    Data = alta.Data,
                    Motivo = alta.Motivo,
                    Destino = alta.Destino
                }
            ).ToListAsync();

            return Ok(altasDetalhes);
        }

        [HttpGet("{utenteId}/{funcionarioId}")]
        public async Task<ActionResult<Alta>> ObterAlta(int utentesId, int funcionarioId)
        {
            IQueryable<Alta> query = _context.Alta;
            query = query.Where(d => d.UtentesId == utentesId && d.FuncionariosId == funcionarioId);


            var altasDetalhes = await (
                from alta in query
                join utente in _context.Utentes on alta.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on alta.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                select new
                {
                    UtentesId = alta.UtentesId,
                    Utentes = utente.Nome,
                    FuncionarioId = alta.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    Data = alta.Data,
                    Motivo = alta.Motivo,
                    Destino = alta.Destino
                }
            ).ToListAsync();

            return Ok(altasDetalhes);
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
