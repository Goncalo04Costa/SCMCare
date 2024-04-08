using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaCorrenteMedicamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContaCorrenteMedicamentoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContaCorrenteMedicamento>>> obterTodasContasCorrentes(
            int? idMin = null, int? idMax = null,
            int? medicamentosId = null,
            int? pedidosMedicamentoId = null,
            int? utentesId = null,
            int? funcionariosId = null,
            DateTime? dataMin = null, DateTime? dataMax = null,
            bool tipo = false,
            int? quantidadeMovimentoMin = null, int? quantidadeMovimentoMax = null,
            string observacoesMin = null, string observacoesMax = null)
        {
            IQueryable<ContaCorrenteMedicamento> query = _context.ContaCorrenteMedicamento;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (medicamentosId.HasValue)
            {
                query = query.Where(d => d.MedicamentosId == medicamentosId.Value);
            }

            if (pedidosMedicamentoId.HasValue)
            {
                query = query.Where(d => d.PedidosMedicamentoId == pedidosMedicamentoId.Value);
            }

            if (funcionariosId.HasValue)
            {
                query = query.Where(d => d.FuncionariosId == funcionariosId.Value);
            }

            if (utentesId.HasValue)
            {
                query = query.Where(d => d.UtentesId == utentesId.Value);
            }

            if (dataMin.HasValue)
            {
                query = query.Where(d => d.Data <= dataMin.Value);
            }

            if (dataMax.HasValue)
            {
                query = query.Where(d => d.Data >= dataMax.Value);
            }

            query = query.Where(d => d.Tipo == tipo);

            if (quantidadeMovimentoMin.HasValue)
            {
                query = query.Where(d => d.QuantidadeMovimento >= quantidadeMovimentoMin);
            }

            if (quantidadeMovimentoMax.HasValue)
            {
                query = query.Where(d => d.QuantidadeMovimento <= quantidadeMovimentoMax.Value);
            }

            if (!string.IsNullOrEmpty(observacoesMin))
            {
                query = query.Where(d => d.Observacoes.CompareTo(observacoesMin) >= 0);
            }

            if (!string.IsNullOrEmpty(observacoesMax))
            {
                query = query.Where(d => d.Observacoes.CompareTo(observacoesMax + "ZZZ") <= 0);
            }

            var dados = await query.ToListAsync();
            return dados;
        }

        [HttpGet("{id")]
        public async Task<ActionResult<ContaCorrenteMedicamento>> obterContaCorrente(int id)
        {
            var dado = await _context.ContaCorrenteMedicamento.FirstOrDefaultAsync(dado => dado.Id == id);

            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<ContaCorrenteMedicamento>> InserirContaCorrente([FromBody] ContaCorrenteMedicamento contaCorrente)
        {
            if (contaCorrente == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.ContaCorrenteMedicamento.Add(contaCorrente);
            await _context.SaveChangesAsync();

            return Ok("Conta corrente adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaContaCorrente(int id, [FromBody] ContaCorrenteMedicamento novaContaCorrente)
        {
            var contaCorrente = await _context.ContaCorrenteMedicamento.FirstOrDefaultAsync(d => d.Id == id);
            if (contaCorrente == null)
            {
                return NotFound($"Não foi possível encontrar a conta corrente com o ID {id}");
            }

            contaCorrente.Fatura = novaContaCorrente.Fatura;
            contaCorrente.MedicamentosId = novaContaCorrente.MedicamentosId;
            contaCorrente.PedidosMedicamentoId = novaContaCorrente.PedidosMedicamentoId;
            contaCorrente.FuncionariosId = novaContaCorrente.FuncionariosId;
            contaCorrente.UtentesId = novaContaCorrente.UtentesId;
            contaCorrente.Data = novaContaCorrente.Data;
            contaCorrente.Tipo = novaContaCorrente.Tipo;
            contaCorrente.QuantidadeMovimento = novaContaCorrente.QuantidadeMovimento;
            contaCorrente.Observacoes = novaContaCorrente.Observacoes;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizada a conta com Id {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveContaCorrente(int id)
        {
            var contaCorrente = await _context.ContaCorrenteMedicamento.FirstOrDefaultAsync(d => d.Id == id);
            if (contaCorrente == null)
            {
                return NotFound($"Não foi possível encontrar a conta corrente com o ID {id}");
            }

            _context.ContaCorrenteMedicamento.Remove(contaCorrente);
            await _context.SaveChangesAsync();

            return Ok($"Foi removida a conta corrente com o ID {id}");
        }
    }
}
