using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposPagamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposPagamentoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoPagamento>>> ObterTodosTiposPagamento(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoPagamento> query = _context.TiposPagamento;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(descMin))
            {
                query = query.Where(d => d.Descricao.CompareTo(descMin) >= 0);
            }

            if (!string.IsNullOrEmpty(descMax))
            {
                query = query.Where(d => d.Descricao.CompareTo(descMax + "ZZZ") <= 0);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoPagamento>> ObterTipo(int id)
        {
            var dado = await _context.TiposPagamento.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoPagamento>> InserirTipo([FromBody] TipoPagamento tipoPagamento)
        {
            if (tipoPagamento == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TiposPagamento.Add(tipoPagamento);
            await _context.SaveChangesAsync();

            return Ok("Tipo de pagamento adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoPagamento(int id, [FromBody] TipoPagamento novoTipoPagamento)
        {
            var tipoPagamento = await _context.TiposPagamento.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoPagamento == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de pagamento com o ID {id}");
            }

            tipoPagamento.Descricao = novoTipoPagamento.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de pagamento com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoPagamento(int id)
        {
            var tipoPagamento = await _context.TiposPagamento.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoPagamento == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de pagamento com o ID {id}");
            }

            _context.TiposPagamento.Remove(tipoPagamento);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de pagamento com o ID {id}");
        }
    }
}
