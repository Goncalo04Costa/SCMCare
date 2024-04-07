using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

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
        public async Task<ActionResult<IEnumerable<MedicamentoPrescricao>>> ObterTodasMedicamentoPrescricao()
        {
            var medicamentoPrescricao = await _context.MedicamentoPrescricao.ToListAsync();
            return Ok(medicamentoPrescricao);
        }

        [HttpGet("{PrescricoesId}/{MedicamentosId}")]
        public async Task<ActionResult<MedicamentoPrescricao>> ObterMedicamentoPrescricao(int PrescricoesId, int MedicamentosId)
        {
            var medicamentoPrescricao = await _context.MedicamentoPrescricao.FirstOrDefaultAsync(a => a.PrescricoesId == PrescricoesId && a.MedicamentosId == MedicamentosId);

            if (medicamentoPrescricao = null)
            {
                return NotFound();
            }
            return Ok(medicamentoPrescricao);
        }

        [HttpPost]
        public async Task<ActionResult<MedicamentoPrescricao>> InserirMedicamentoPrescricao([FromBody] MedicamentoPrescricao medicamentoPrescricao)
        {
            if (medicamentoPrescricao == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.MedicamentoPrescricao.Add(medicamentoPrescricao);
            await _context.SaveChangesAsync();

            return Ok("medicamentoPrescricao adicionada com sucesso");
        }

        [HttpPut("{PrescricoesId}/{MedicamentosId}")]
        public async Task<IActionResult> AtualizaMedicamentoPrescricao(int PrescricoesId, int MedicamentosId, [FromBody] MedicamentoPrescricao novaMedicamentoPrescricao)
        {
            var medicamentoPrescricao = await _context.MedicamentoPrescricao.FirstOrDefaultAsync(a => a.PrescricoesId == PrescricoesId && a.MedicamentosId == MedicamentosId);

            if (medicamentoPrescricao == null)
            {
                return NotFound($"Não foi possível encontrar a medicamentoPrescricao com a prescricao ID {PrescricoesId} e medicamento ID {MedicamentosId}");
            }

            medicamentoPrescricao.QuantidadePIntervalo = novaMedicamentoPrescricao.QuantidadePIntervalo;
            medicamentoPrescricao.IntervaloHoras = novaMedicamentoPrescricao.IntervaloHoras;
            medicamentoPrescricao.Instrucoes = novaMedicamentoPrescricao.Instrucoes;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizada a medicamentoPrescricao com a prescricao ID {PrescricoesId} e medicamento ID {MedicamentosId}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{PrescricoesId}/{MedicamentosId}")]
        public async Task<IActionResult> RemoveMedicamentoPrescricao(int PrescricoesId, int MedicamentosId)
        {
            var medicamentoPrescricao = await _context.MedicamentoPrescricao.FirstOrDefaultAsync(a => a.PrescricoesId == PrescricoesId && a.MedicamentosId == MedicamentosId);

            if (medicamentoPrescricao == null)
            {
                return NotFound($"Não foi possível encontrar o medicamentoPrescricao com a prescricao ID {PrescricoesId} e medicamento ID {MedicamentosId}");
            }

            _context.MedicamentoPrescricao.Remove(medicamentoPrescricao);
            await _context.SaveChangesAsync();

            return Ok($"Foi removida a medicamentoPrescricao com a prescricao ID {PrescricoesId} e medicamento ID {MedicamentosId}");
        }
    }
}