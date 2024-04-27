using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposAvaliacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposAvaliacaoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoAvaliacao>>> ObterTodosTiposAvaliacao(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoAvaliacao> query = _context.TipoAvaliacao;

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
        public async Task<ActionResult<TipoAvaliacao>> ObterTipo(int id)
        {
            var dado = await _context.TipoAvaliacao.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoAvaliacao>> InserirTipo([FromBody] TipoAvaliacao tipoAvaliacao)
        {
            if (tipoAvaliacao == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TipoAvaliacao.Add(tipoAvaliacao);
            await _context.SaveChangesAsync();

            return Ok("Tipo de avaliação adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoAvaliacao(int id, [FromBody] TipoAvaliacao novoTipoAvaliacao)
        {
            var tipoAvaliacao = await _context.TipoAvaliacao.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoAvaliacao == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de avaliação com o ID {id}");
            }

            tipoAvaliacao.Descricao = novoTipoAvaliacao.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de avaliação com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoAvaliacao(int id)
        {
            var tipoAvaliacao = await _context.TipoAvaliacao.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoAvaliacao == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de avaliação com o ID {id}");
            }

            _context.TipoAvaliacao.Remove(tipoAvaliacao);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de avaliação com o ID {id}");
        }
    }
}
