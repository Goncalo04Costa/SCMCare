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
        public async Task<ActionResult<IEnumerable<Medicamento>>> ObterTodosMedicamentos(
            int? idMin = null, int? idMax = null,
            string? nomeMin = null, string? nomeMax = null,
            bool ativo0 = false, bool ativo1 = false)
        {
            IQueryable<Medicamento> query = _context.Medicamentos;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMin) >= 0);
            }

            if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMax + "ZZZ") <= 0);
            }

            if (ativo0 && !ativo1)
            {
                query = query.Where(d => !d.Ativo);
            }

            else if (!ativo0 && ativo1)
            {
                query = query.Where(d => d.Ativo);
            }

            var medicamentosDetalhes = await (
                from medicamentos in query
                select new
                {
                    Id = medicamentos.Id,
                    Nome = medicamentos.Nome,
                    Descricao = medicamentos.Descricao,
                    Quantidade = _context.ContaCorrenteMedicamentos
                        .Where(m => m.MedicamentosId == medicamentos.Id)
                        .Sum(m => m.Tipo ? m.QuantidadeMovimento : -m.QuantidadeMovimento),
                    Limite = medicamentos.Limite,
                    Ativo = medicamentos.Ativo
                }
            ).ToListAsync();

            return Ok(medicamentosDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento>> ObterMedicamento(int id)
        {
            IQueryable<Medicamento> query = _context.Medicamentos;
            query = query.Where(d => d.Id == id);

            var medicamentosDetalhes = await (
                from medicamentos in query
                select new
                {
                    Id = medicamentos.Id,
                    Nome = medicamentos.Nome,
                    Descricao = medicamentos.Descricao,
                    Quantidade = _context.ContaCorrenteMedicamentos
                        .Where(m => m.MedicamentosId == medicamentos.Id)
                        .Sum(m => m.Tipo ? m.QuantidadeMovimento : -m.QuantidadeMovimento),
                    Limite = medicamentos.Limite,
                    Ativo = medicamentos.Ativo
                }
            ).ToListAsync();

            return Ok(medicamentosDetalhes);
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
        public async Task<ActionResult<IEnumerable<Medicamento>>> ObterMedicamentoRisco()
        {
            try
            {
                IQueryable<Medicamento> query = _context.Medicamentos;

                var medicamentosDetalhes = await (
                    from medicamentos in query
                    let quantidadeAtual = _context.ContaCorrenteMateriais
                        .Where(m => m.MateriaisId == medicamentos.Id)
                        .Sum(m => m.Tipo ? m.QuantidadeMovimento : -m.QuantidadeMovimento)
                    where quantidadeAtual < medicamentos.Limite
                    select new
                    {
                        Id = medicamentos.Id,
                        Nome = medicamentos.Nome,
                        Descricao = medicamentos.Descricao,
                        Quantidade = quantidadeAtual,
                        Limite = medicamentos.Limite,
                        Ativo = medicamentos.Ativo
                    }
                ).ToListAsync();

                return Ok(medicamentosDetalhes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao obter medicamentos em risco: {ex.Message}");
            }
        }
    }
}
