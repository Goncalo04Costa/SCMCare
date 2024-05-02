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
        public async Task<ActionResult<IEnumerable<Prescricao>>> ObterTodasPrescricoes(
            int? idMin = null, int? idMax = null,
            int? utenteId = null,
            DateTime? dataInicioMin = null, DateTime? dataInicioMax = null,
            DateTime? dataFimMin = null, DateTime? dataFimMax = null)
        {
            IQueryable<Prescricao> query = _context.Prescricoes;

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


            var prescricoesDetalhes = await (
                from prescricao in query
                join utente in _context.Utentes on prescricao.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                select new
                {
                    Id = prescricao.Id,
                    UtenteId = prescricao.UtentesId,
                    Utente = utente.Nome,

                    Medicamentos = _context.MedicamentosPrescricao
                        .Where(cp => cp.PrescricoesId == prescricao.Id)
                        .Join(
                            _context.Medicamentos,
                            cp => cp.MedicamentosId,
                            m => m.Id,
                            (cp, m) => new
                            {
                                MedicamentoId = m.Id,
                                Medicamento = m.Nome,
                                QuantidadePI = cp.QuantidadePIntervalo,
                                Intervalo = cp.IntervaloHoras,
                                Intrucoes = cp.Instrucoes
                            }
                        )
                        .ToList(),

                    DataInicio = prescricao.DataInicio,
                    DataFim = prescricao.DataFim,
                    Observacoes = prescricao.Observacoes
                }
            ).ToListAsync();

            return Ok(prescricoesDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Prescricao>> ObterPrescricao(int id)
        {
            IQueryable<Prescricao> query = _context.Prescricoes;
            query = query.Where(d => d.Id == id);

            var prescricaoDetalhes = await (
                from prescricao in query
                join utente in _context.Utentes on prescricao.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                select new
                {
                    Id = prescricao.Id,
                    UtenteId = prescricao.UtentesId,
                    Utente = utente.Nome,

                    Medicamentos = _context.MedicamentosPrescricao
                        .Where(cp => cp.PrescricoesId == prescricao.Id)
                        .Join(
                            _context.Medicamentos,
                            cp => cp.MedicamentosId,
                            m => m.Id,
                            (cp, m) => new
                            {
                                MedicamentoId = m.Id,
                                Medicamento = m.Nome,
                                QuantidadePI = cp.QuantidadePIntervalo,
                                Intervalo = cp.IntervaloHoras,
                                Intrucoes = cp.Instrucoes
                            }
                        )
                        .ToList(),

                    DataInicio = prescricao.DataInicio,
                    DataFim = prescricao.DataFim,
                    Observacoes = prescricao.Observacoes
                }
            ).ToListAsync();

            return Ok(prescricaoDetalhes);
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
