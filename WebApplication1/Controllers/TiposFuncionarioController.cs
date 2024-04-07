using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposFuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposFuncionarioController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoFuncionario>>> ObterTodosTiposFuncionario(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoFuncionario> query = _context.TiposFuncionario;

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
        public async Task<ActionResult<TipoFuncionario>> ObterTipo(int id)
        {
            var dado = await _context.TiposFuncionario.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoFuncionario>> InserirTipo([FromBody] TipoFuncionario tipoFuncionario)
        {
            if (tipoFuncionario == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TiposFuncionario.Add(tipoFuncionario);
            await _context.SaveChangesAsync();

            return Ok("Tipo de funcionário adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoFuncionario(int id, [FromBody] TipoFuncionario novoTipoFuncionario)
        {
            var tipoFuncionario = await _context.TiposFuncionario.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoFuncionario == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de funcionário com o ID {id}");
            }

            tipoFuncionario.Descricao = novoTipoFuncionario.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de funcionário com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoFuncionario(int id)
        {
            var tipoFuncionario = await _context.TiposFuncionario.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoFuncionario == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de funcionário com o ID {id}");
            }

            _context.TiposFuncionario.Remove(tipoFuncionario);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de funcionário com o ID {id}");
        }
    }
}
