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
        public async Task<ActionResult<IEnumerable<FornecedorMedicamento>>> ObterTodosFornecedorMedicamento()
        {
            var fornecedorMedicamento = await _context.FornecedoresMedicamento.ToListAsync();
            return Ok(fornecedorMedicamento);
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