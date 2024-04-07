using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposQuartoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposQuartoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoQuarto>>> ObterTodosTiposQuarto(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoQuarto> query = _context.TiposQuarto;

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
        public async Task<ActionResult<TipoQuarto>> ObterTipo(int id)
        {
            var dado = await _context.TiposQuarto.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoQuarto>> InserirTipo([FromBody] TipoQuarto tipoQuarto)
        {
            if (tipoQuarto == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TiposQuarto.Add(tipoQuarto);
            await _context.SaveChangesAsync();

            return Ok("Tipo de quarto adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoQuarto(int id, [FromBody] TipoQuarto novoTipoQuarto)
        {
            var tipoQuarto = await _context.TiposQuarto.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoQuarto == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de quarto com o ID {id}");
            }

            tipoQuarto.Descricao = novoTipoQuarto.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de quarto com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoQuarto(int id)
        {
            var tipoQuarto = await _context.TiposQuarto.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoQuarto == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de quarto com o ID {id}");
            }

            _context.TiposQuarto.Remove(tipoQuarto);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de quarto com o ID {id}");
        }
    }
}
