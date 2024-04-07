using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposMaterialController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposMaterialController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoMaterial>>> ObterTodosTiposMaterial(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoMaterial> query = _context.TiposMaterial;

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
        public async Task<ActionResult<TipoMaterial>> ObterTipo(int id)
        {
            var dado = await _context.TiposMaterial.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoMaterial>> InserirTipo([FromBody] TipoMaterial tipoMaterial)
        {
            if (tipoMaterial == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TiposMaterial.Add(tipoMaterial);
            await _context.SaveChangesAsync();

            return Ok("Tipo de material adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoMaterial(int id, [FromBody] TipoMaterial novoTipoMaterial)
        {
            var tipoMaterial = await _context.TiposMaterial.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoMaterial == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de material com o ID {id}");
            }

            tipoMaterial.Descricao = novoTipoMaterial.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de material com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoMaterial(int id)
        {
            var tipoMaterial = await _context.TiposMaterial.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoMaterial == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de material com o ID {id}");
            }

            _context.TiposMaterial.Remove(tipoMaterial);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de material com o ID {id}");
        }
    }
}
