using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposAlergiaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposAlergiaController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoAlergia>>> ObterTodosTiposAlergia(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoAlergia> query = _context.TiposAlergia;

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
        public async Task<ActionResult<TipoAlergia>> ObterTipo(int id)
        {
            var dado = await _context.TiposAlergia.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoAlergia>> InserirTipo([FromBody] TipoAlergia tipoAlergia)
        {
            if (tipoAlergia == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TiposAlergia.Add(tipoAlergia);
            await _context.SaveChangesAsync();

            return Ok("Tipo de alergia adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoAlergia(int id, [FromBody] TipoAlergia novoTipoAlergia)
        {
            var tipoAlergia = await _context.TiposAlergia.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoAlergia == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de alergia com o ID {id}");
            }

            tipoAlergia.Descricao = novoTipoAlergia.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de alergia com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoAlergia(int id)
        {
            var tipoAlergia = await _context.TiposAlergia.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoAlergia == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de alergia com o ID {id}");
            }

            _context.TiposAlergia.Remove(tipoAlergia);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de alergia com o ID {id}");
        }
    }
}
