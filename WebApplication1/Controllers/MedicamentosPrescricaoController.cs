using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentosPrescricaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicamentosPrescricaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicamentoPrescricao>>> ObterTodosMedicamentoPrescricao(
            int? prescricaoId = null,
            int? medicamentoId = null)
        {
            IQueryable<MedicamentoPrescricao> query = _context.MedicamentosPrescricao;

            if (prescricaoId.HasValue)
            {
                query = query.Where(d => d.PrescricoesId == prescricaoId.Value);
            }

            if (medicamentoId.HasValue)
            {
                query = query.Where(d => d.MedicamentosId == medicamentoId.Value);
            }


            var medicamentoPrescricaoDetalhes = await (
                from medPre in query
                join medicamento in _context.Medicamentos on medPre.MedicamentosId equals medicamento.Id into mG
                from medicamento in mG.DefaultIfEmpty()
                select new
                {
                    PrescricaoId = medPre.PrescricoesId,
                    MedicamentoId = medPre.MedicamentosId,
                    Medicamento = medicamento.Nome,
                    QuantidadePI = medPre.QuantidadePIntervalo,
                    Intervalo = medPre.IntervaloHoras,
                    Intrucoes = medPre.Instrucoes
                }
            ).ToListAsync();

            return Ok(medicamentoPrescricaoDetalhes);
        }

        [HttpGet("{PrescricoesId}/{MedicamentosId}")]
        public async Task<ActionResult<MedicamentoPrescricao>> ObterMedicamentoPrescricao(int PrescricoesId, int MedicamentosId)
        {
            IQueryable<MedicamentoPrescricao> query = _context.MedicamentosPrescricao;
            query = query.Where(d => d.PrescricoesId == PrescricoesId && d.MedicamentosId == MedicamentosId);

            var medicamentoPrescricaoDetalhes = await (
                from medPre in query
                join medicamento in _context.Medicamentos on medPre.MedicamentosId equals medicamento.Id into mG
                from medicamento in mG.DefaultIfEmpty()
                select new
                {
                    PrescricaoId = medPre.PrescricoesId,
                    MedicamentoId = medPre.MedicamentosId,
                    Medicamento = medicamento.Nome,
                    QuantidadePI = medPre.QuantidadePIntervalo,
                    Intervalo = medPre.IntervaloHoras,
                    Intrucoes = medPre.Instrucoes
                }
            ).ToListAsync();

            return Ok(medicamentoPrescricaoDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<MedicamentoPrescricao>> InserirMedicamentoPrescricao([FromBody] MedicamentoPrescricao medicamentoPrescricao)
        {
            if (medicamentoPrescricao == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.MedicamentosPrescricao.Add(medicamentoPrescricao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterMedicamentoPrescricao), new { PrescricoesId = medicamentoPrescricao.PrescricoesId, MedicamentosId = medicamentoPrescricao.MedicamentosId }, medicamentoPrescricao);
        }

        [HttpPut("{PrescricoesId}/{MedicamentosId}")]
        public async Task<IActionResult> AtualizaMedicamentoPrescricao(int PrescricoesId, int MedicamentosId, [FromBody] MedicamentoPrescricao novaMedicamentoPrescricao)
        {
            var medicamentoPrescricao = await _context.MedicamentosPrescricao.FirstOrDefaultAsync(a => a.PrescricoesId == PrescricoesId && a.MedicamentosId == MedicamentosId);

            if (medicamentoPrescricao == null)
            {
                return NotFound($"Não foi possível encontrar a medicamentoPrescricao com a prescricao ID {PrescricoesId} e medicamento ID {MedicamentosId}");
            }

            medicamentoPrescricao.QuantidadePIntervalo = novaMedicamentoPrescricao.QuantidadePIntervalo;
            medicamentoPrescricao.IntervaloHoras = novaMedicamentoPrescricao.IntervaloHoras;
            medicamentoPrescricao.Instrucoes = novaMedicamentoPrescricao.Instrucoes;

            await _context.SaveChangesAsync();

            return Ok($"Foi atualizada a medicamentoPrescricao com a prescricao ID {PrescricoesId} e medicamento ID {MedicamentosId}");
        }

        [HttpDelete("{PrescricoesId}/{MedicamentosId}")]
        public async Task<IActionResult> RemoveMedicamentoPrescricao(int PrescricoesId, int MedicamentosId)
        {
            var medicamentoPrescricao = await _context.MedicamentosPrescricao.FirstOrDefaultAsync(a => a.PrescricoesId == PrescricoesId && a.MedicamentosId == MedicamentosId);

            if (medicamentoPrescricao == null)
            {
                return NotFound($"Não foi possível encontrar o medicamentoPrescricao com a prescricao ID {PrescricoesId} e medicamento ID {MedicamentosId}");
            }

            _context.MedicamentosPrescricao.Remove(medicamentoPrescricao);
            await _context.SaveChangesAsync();

            return Ok($"Foi removida a medicamentoPrescricao com a prescricao ID {PrescricoesId} e medicamento ID {MedicamentosId}");
        }
    }
}
