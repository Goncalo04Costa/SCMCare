using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TiposEquipamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposEquipamentoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoEquipamento>>> ObterTodosTiposEquipamento(
            int? idMin = null, int? idMax = null,
            string descMin = null, string descMax = null)
        {
            IQueryable<TipoEquipamento> query = _context.TiposEquipamento;

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
        public async Task<ActionResult<TipoEquipamento>> ObterTipo(int id)
        {
            var dado = await _context.TiposEquipamento.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<TipoEquipamento>> InserirTipo([FromBody] TipoEquipamento tipoEquipamento)
        {
            if (tipoEquipamento == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.TiposEquipamento.Add(tipoEquipamento);
            await _context.SaveChangesAsync();

            return Ok("Tipo de equipamento adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTipoEquipamento(int id, [FromBody] TipoEquipamento novoTipoEquipamento)
        {
            var tipoEquipamento = await _context.TiposEquipamento.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoEquipamento == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de equipamento com o ID {id}");
            }

            tipoEquipamento.Descricao = novoTipoEquipamento.Descricao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o tipo de equipamento com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTipoEquipamento(int id)
        {
            var tipoEquipamento = await _context.TiposEquipamento.FirstOrDefaultAsync(d => d.Id == id);
            if (tipoEquipamento == null)
            {
                return NotFound($"Não foi possível encontrar o tipo de equipamento com o ID {id}");
            }

            _context.TiposEquipamento.Remove(tipoEquipamento);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido tipo de equipamento com o ID {id}");
        }
    }
}
