using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvariasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvariasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avaria>>> ObterTodasAvarias()
        {
            var avarias = await _context.Avarias.ToListAsync();
            return Ok(avarias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Avaria>> ObterAvaria(int id)
        {
            var avaria = await _context.Avarias.FindAsync(id);

            if (avaria == null)
            {
                return NotFound();
            }

            return Ok(avaria);
        }

        [HttpPost]
        public async Task<ActionResult<Avaria>> InserirAvaria([FromBody] Avaria avaria)
        {
            if (avaria == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Avarias.Add(avaria);
            await _context.SaveChangesAsync();

            return Ok("Avaria adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAvaria(int id, [FromBody] Avaria novaAvaria)
        {
            var avaria = await _context.Avarias.FindAsync(id);

            if (avaria == null)
            {
                return NotFound($"Não foi possível encontrar a avaria com o ID {id}");
            }

            avaria.Data = novaAvaria.Data;
            avaria.EquipamentosId = novaAvaria.EquipamentosId;
            avaria.Descricao = novaAvaria.Descricao;
            avaria.Estado = novaAvaria.Estado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Avaria atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverAvaria(int id)
        {
            var avaria = await _context.Avarias.FindAsync(id);

            if (avaria == null)
            {
                return NotFound($"Avaria com o ID {id} não encontrada");
            }

            _context.Avarias.Remove(avaria);
            await _context.SaveChangesAsync();

            return Ok($"Avaria com o ID {id} removida com sucesso");
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarDataAvaria(int id, DateTime data)
        {
            var avaria = await _context.Avarias.FindAsync(id);

            if (avaria == null)
            {
                return NotFound($"Não foi possível encontrar a avaria com o ID {id}");
            }

            avaria.Data = data;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Avaria atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
