using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlanosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plano>>> ObterTodosPlanos(
            int? idMin = null, int? idMax = null,
            int? utenteId = null,
            DateTime? dataInicioMin = null, DateTime? dataInicioMax = null,
            DateTime? dataFimMin = null, DateTime? dataFimMax = null)
        {
            IQueryable<Plano> query = _context.Planos;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (utenteId.HasValue)
            {
                query = query.Where(d => d.UtentesId == utenteId.Value);
            }

            if (dataInicioMin.HasValue)
            {
                query = query.Where(d => d.DataInicio >= dataInicioMin.Value);
            }

            if (dataInicioMax.HasValue)
            {
                query = query.Where(d => d.DataInicio <= dataInicioMax.Value);
            }

            if (dataFimMin.HasValue)
            {
                query = query.Where(d => d.DataFim >= dataFimMin.Value);
            }

            if (dataFimMax.HasValue)
            {
                query = query.Where(d => d.DataFim <= dataFimMax.Value);
            }


            var planosDetalhes = await (
                from plano in query
                join utente in _context.Utentes on plano.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                select new
                {
                    Id = plano.Id,
                    UtenteId = plano.UtentesId,
                    Utente = utente.Nome,

                    Materiais = _context.MateriaisPlano
                        .Where(cp => cp.PlanosId == plano.Id)
                        .Join(
                            _context.Materiais,
                            cp => cp.MateriaisId,
                            m => m.Id,
                            (cp, m) => new
                            {
                                MaterialId = m.Id,
                                Material = m.Nome,
                                QuantidadePI = cp.QuantidadePIntervalo,
                                Intervalo = cp.IntervaloHoras,
                                Intrucoes = cp.Instrucoes
                            }
                        )
                        .ToList(),

                    DataInicio = plano.DataInicio,
                    DataFim = plano.DataFim,
                    Observacoes = plano.Observacoes
                }
            ).ToListAsync();

            return Ok(planosDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plano>> ObterPlano(int id)
        {
            IQueryable<Plano> query = _context.Planos;
            query = query.Where(d => d.Id == id);

            var planoDetalhes = await (
                from plano in query
                join utente in _context.Utentes on plano.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                select new
                {
                    Id = plano.Id,
                    UtenteId = plano.UtentesId,
                    Utente = utente.Nome,

                    Materiais = _context.MateriaisPlano
                        .Where(cp => cp.PlanosId == plano.Id)
                        .Join(
                            _context.Materiais,
                            cp => cp.MateriaisId,
                            m => m.Id,
                            (cp, m) => new
                            {
                                MaterialId = m.Id,
                                Material = m.Nome,
                                QuantidadePI = cp.QuantidadePIntervalo,
                                Intervalo = cp.IntervaloHoras,
                                Intrucoes = cp.Instrucoes
                            }
                        )
                        .ToList(),

                    DataInicio = plano.DataInicio,
                    DataFim = plano.DataFim,
                    Observacoes = plano.Observacoes
                }
            ).ToListAsync();

            return Ok(planoDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Plano>> InserirPlano([FromBody] Plano plano)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Planos.Add(plano);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPlano), new { id = plano.Id }, plano);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPlano(int id, [FromBody] Plano plano)
        {
            if (id != plano.Id)
            {
                return BadRequest();
            }

            _context.Entry(plano).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanoExists(id))
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
        public async Task<IActionResult> RemoverPlano(int id)
        {
            var plano = await _context.Planos.FindAsync(id);
            if (plano == null)
            {
                return NotFound();
            }

            _context.Planos.Remove(plano);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanoExists(int id)
        {
            return _context.Planos.Any(e => e.Id == id);
        }
    }
}
