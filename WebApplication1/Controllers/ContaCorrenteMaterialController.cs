using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaCorrenteMaterialController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContaCorrenteMaterialController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContaCorrenteMaterial>>> obterTodasContasCorrentes(
            int? idMin = null, int? idMax = null,
            int? materiaisId = null,
            int? pedidosMaterialId = null,
            int? utentesId = null,
            int? funcionariosId = null,
            DateTime? dataMin = null, DateTime? dataMax = null,
            bool tipo0 = false, bool tipo1 = false,
            int? quantidadeMovimentoMin = null, int? quantidadeMovimentoMax = null,
            string observacoesMin = null, string observacoesMax = null)
        {
            IQueryable<ContaCorrenteMaterial> query = _context.ContaCorrenteMateriais;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (materiaisId.HasValue)
            {
                query = query.Where(d => d.MateriaisId == materiaisId.Value);
            }

            if (pedidosMaterialId.HasValue)
            {
                query = query.Where(d => d.PedidosMaterialId == pedidosMaterialId.Value);
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

            if (tipo0 && !tipo1)
            {
                query = query.Where(d => !d.Tipo);
            }

            else if (!tipo0 && tipo1)
            {
                query = query.Where(d => d.Tipo);
            }

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

        [HttpGet("{id}")]
        public async Task<ActionResult<ContaCorrenteMaterial>> obterContaCorrente(int id)
        {
            var dado = await _context.ContaCorrenteMateriais.FirstOrDefaultAsync(dado => dado.Id == id);

            if (dado == null)
            { 
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<ContaCorrenteMaterial>> InserirContaCorrente([FromBody] ContaCorrenteMaterial contaCorrente)
        {
            if (contaCorrente == null) 
            {
                return BadRequest("Objeto inválido");
            }

            _context.ContaCorrenteMateriais.Add(contaCorrente);
            await _context.SaveChangesAsync();

            return Ok("Adicionado novo registo na conta corrente");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaContaCorrente(int id, [FromBody] ContaCorrenteMaterial novaContaCorrente)
        {
            var contaCorrente = await _context.ContaCorrenteMateriais.FirstOrDefaultAsync(d  => d.Id == id);
            if (contaCorrente == null)
            {
                return NotFound($"Não foi possível encontrar o registo com o Id {id}");
            }

            contaCorrente.Fatura = novaContaCorrente.Fatura;
            contaCorrente.MateriaisId = novaContaCorrente.MateriaisId;
            contaCorrente.PedidosMaterialId = novaContaCorrente.PedidosMaterialId;
            contaCorrente.FuncionariosId = novaContaCorrente.FuncionariosId;
            contaCorrente.UtentesId = novaContaCorrente.UtentesId;
            contaCorrente.Data = novaContaCorrente.Data;
            contaCorrente.Tipo = novaContaCorrente.Tipo;
            contaCorrente.QuantidadeMovimento = novaContaCorrente.QuantidadeMovimento;
            contaCorrente.Observacoes = novaContaCorrente.Observacoes;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o registo com Id {id}"); 
            }
            catch ( Exception e) 
            {
                throw e;
            }
        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> RemoveContaCorrente(int id)
        {
            var contaCorrente = await _context.ContaCorrenteMateriais.FirstOrDefaultAsync(d => d.Id == id);
            if (contaCorrente == null)
            {
                return NotFound($"Não foi possível encontrar o registo com o ID {id}");
            }

            _context.ContaCorrenteMateriais.Remove(contaCorrente);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o registo com o Id {id}");
        }
    }
}
