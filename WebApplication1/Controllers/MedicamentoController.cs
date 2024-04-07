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
    public class MedicamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicamentosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicamento>>> ObterTodosMedicamentos()
        {
            return await _context.Medicamentos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento>> ObterMedicamento(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);

            if (medicamento == null)
            {
                return NotFound();
            }

            return medicamento;
        }

        [HttpPost]
        public async Task<ActionResult<Medicamento>> InserirMedicamento([FromBody] Medicamento medicamento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Medicamentos.Add(medicamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterMedicamento), new { id = medicamento.Id }, medicamento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarMedicamento(int id, [FromBody] Medicamento medicamento)
        {
            if (id != medicamento.Id)
            {
                return BadRequest();
            }

            _context.Entry(medicamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicamentoExists(id))
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
        public async Task<IActionResult> RemoverMedicamento(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null)
            {
                return NotFound();
            }

            _context.Medicamentos.Remove(medicamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicamentoExists(int id)
        {
            return _context.Medicamentos.Any(e => e.Id == id);
        }

        [HttpGet("emrisco")]
        public async Task<ActionResult<IEnumerable<Medicamento>>> ObterMedicamentoRisco(int limite)
        {
            try
            {
                var medicamentoRisco = await _context.Medicamentos
                    .Where(m => m.QuantidadeAtual < m.Limite)
                    .ToListAsync();

                return Ok(medicamentoRisco);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao obter medicamentos em risco: {ex.Message}");
            }
        }
    }
}
