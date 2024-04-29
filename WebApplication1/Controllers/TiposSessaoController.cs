using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposSessaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposSessaoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoSessao>>> ObterTodosTiposSessao(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoSessao> query = _context.TiposSessao;

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
        public async Task<ActionResult<TipoSessao>> ObterTipo(int id)
        {
            var dado = await _context.TiposSessao.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoSessao>> InserirTipo([FromBody] TipoSessao tipoSessao)
        {
            if (tipoSessao == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TiposSessao.Add(tipoSessao);
            await _context.SaveChangesAsync();

            return Ok("Tipo de sessão adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoSessao(int id, [FromBody] TipoSessao novoTipoSessao)
        {
            var tipoSessao = await _context.TiposSessao.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoSessao == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de sessão com o ID {id}");
            }

            tipoSessao.Descricao = novoTipoSessao.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de sessão com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoSessao(int id)
        {
            var tipoSessao = await _context.TiposSessao.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoSessao == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de sessão com o ID {id}");
            }

            _context.TiposSessao.Remove(tipoSessao);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de sessão com o ID {id}");
        }
    }
}
