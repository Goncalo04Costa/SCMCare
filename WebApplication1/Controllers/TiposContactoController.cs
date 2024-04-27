using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposContactoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposContactoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoContacto>>> ObterTodosTiposContacto(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoContacto> query = _context.TipoContacto;

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
        public async Task<ActionResult<TipoContacto>> ObterTipo(int id)
        {
            var dado = await _context.TipoContacto.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoContacto>> InserirTipo([FromBody] TipoContacto tipoContacto)
        {
            if (tipoContacto == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TipoContacto.Add(tipoContacto);
            await _context.SaveChangesAsync();

            return Ok("Tipo de contacto adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoContacto(int id, [FromBody] TipoContacto novoTipoContacto)
        {
            var tipoContacto = await _context.TipoContacto.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoContacto == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de contacto com o ID {id}");
            }

            tipoContacto.Descricao = novoTipoContacto.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de contacto com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoContacto(int id)
        {
            var tipoContacto = await _context.TipoContacto.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoContacto == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de contacto com o ID {id}");
            }

            _context.TipoContacto.Remove(tipoContacto);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de contacto com o ID {id}");
        }
    }
}
