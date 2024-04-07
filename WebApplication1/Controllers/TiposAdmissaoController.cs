using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposAdmissaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposAdmissaoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoAdmissao>>> ObterTodosTiposAdmissao(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoAdmissao> query = _context.TiposAdmissao;

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
        public async Task<ActionResult<TipoAdmissao>> ObterTipo(int id)
        {
            var dado = await _context.TiposAdmissao.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoAdmissao>> InserirTipo([FromBody] TipoAdmissao tipoAdmissao)
        {
            if (tipoAdmissao == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TiposAdmissao.Add(tipoAdmissao);
            await _context.SaveChangesAsync();

            return Ok("Tipo de admissão adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoAdmissao(int id, [FromBody] TipoAdmissao novoTipoAdmissao)
        {
            var tipoAdmissao = await _context.TiposAdmissao.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoAdmissao == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de admissão com o ID {id}");
            }

            tipoAdmissao.Descricao = novoTipoAdmissao.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de admissão com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoAdmissao(int id)
        {
            var tipoAdmissao = await _context.TiposAdmissao.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoAdmissao == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de admissão com o ID {id}");
            }

            _context.TiposAdmissao.Remove(tipoAdmissao);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de admissão com o ID {id}");
        }
    }
}
