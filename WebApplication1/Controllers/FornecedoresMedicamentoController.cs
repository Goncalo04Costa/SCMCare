using iText.Kernel.Pdf.Canvas.Wmf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresMedicamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FornecedoresMedicamentoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorMedicamento>>> ObterTodosFornecedorMedicamento(
            int? medicamentoId = null,
            int? fornecedorId = null)
        {
            IQueryable<FornecedorMedicamento> query = _context.FornecedoresMedicamento;

            if (medicamentoId.HasValue)
            {
                query = query.Where(d => d.MedicamentosId == medicamentoId.Value);
            }

            if (fornecedorId.HasValue)
            {
                query = query.Where(d => d.FornecedoresId == fornecedorId.Value);
            }


            var fornecedoresMedicamentosDetalhes = await (
                from forMed in query
                join medicamento in _context.Medicamentos on forMed.MedicamentosId equals medicamento.Id into mG
                from medicamento in mG.DefaultIfEmpty()
                join fornecedor in _context.Fornecedores on forMed.FornecedoresId equals fornecedor.Id into fG
                from fornecedor in fG.DefaultIfEmpty()
                select new
                {
                    MedicamentoId = forMed.MedicamentosId,
                    Medicamento = medicamento.Nome,
                    FornecedorId = forMed.FornecedoresId,
                    Fornecedor = fornecedor.Nome
                }
            ).ToListAsync();

            return Ok(fornecedoresMedicamentosDetalhes);
        }

        [HttpGet("{idMedicamento}/{idFornecedor}")]
        public async Task<ActionResult<FornecedorMedicamento>> ObterFornecedorMedicamento(int idMedicamento, int idFornecedor)
        {
            IQueryable<FornecedorMedicamento> query = _context.FornecedoresMedicamento;
            query = query.Where(d => d.MedicamentosId == idMedicamento && d.FornecedoresId == idFornecedor);


            var fornecedorMedicamentoDetalhes = await (
                from forMed in query
                join medicamento in _context.Medicamentos on forMed.MedicamentosId equals medicamento.Id into mG
                from medicamento in mG.DefaultIfEmpty()
                join fornecedor in _context.Fornecedores on forMed.FornecedoresId equals fornecedor.Id into fG
                from fornecedor in fG.DefaultIfEmpty()
                select new
                {
                    MedicamentoId = forMed.MedicamentosId,
                    Medicamento = medicamento.Nome,
                    FornecedorId = forMed.FornecedoresId,
                    Fornecedor = fornecedor.Nome
                }
            ).ToListAsync();

            return Ok(fornecedorMedicamentoDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorMedicamento>> InserirFornecedorMedicamento([FromBody] FornecedorMedicamento fornecedorMedicamento)
        {
            if (fornecedorMedicamento == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.FornecedoresMedicamento.Add(fornecedorMedicamento);
            await _context.SaveChangesAsync();

            return Ok("fornecedorMedicamento adicionado com sucesso");
        }

        [HttpDelete("{MedicamentosId}/{FornecedoresId}")]
        public async Task<IActionResult> RemoveFornecedorMedicamento(int MedicamentosId, int FornecedoresId)
        {
            var fornecedorMedicamento = await _context.FornecedoresMedicamento.FirstOrDefaultAsync(a => a.MedicamentosId == MedicamentosId && a.FornecedoresId == FornecedoresId);

            if (fornecedorMedicamento == null)
            {
                return NotFound($"Não foi possível encontrar o fornecedorMedicamento com o medicamento ID {MedicamentosId} e o fornecedor ID {FornecedoresId}");
            }

            _context.FornecedoresMedicamento.Remove(fornecedorMedicamento);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o fornecedorMedicamento com o medicamento ID {MedicamentosId} e o fornecedor ID {FornecedoresId}");
        }
    }
}