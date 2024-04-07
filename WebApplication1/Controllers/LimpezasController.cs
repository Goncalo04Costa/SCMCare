using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimpezasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LimpezasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Limpeza>>> ObterTodasLimpezas()
        {
            return await _context.Limpezas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Limpeza>> ObterLimpeza(int id)
        {
            var limpeza = await _context.Limpezas.FindAsync(id);

            if (limpeza == null)
            {
                return NotFound();
            }

            return limpeza;
        }

        [HttpPost]
        public async Task<ActionResult<Limpeza>> InserirLimpeza([FromBody] Limpeza limpeza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Limpezas.Add(limpeza);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterLimpeza), new { id = limpeza.Id }, limpeza);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarLimpeza(int id, [FromBody] Limpeza limpeza)
        {
            if (id != limpeza.Id)
            {
                return BadRequest();
            }

            _context.Entry(limpeza).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LimpezaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverLimpeza(int id)
        {
            var limpeza = await _context.Limpezas.FindAsync(id);
            if (limpeza == null)
            {
                return NotFound();
            }

            _context.Limpezas.Remove(limpeza);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LimpezaExists(int id)
        {
            return _context.Limpezas.Any(e => e.Id == id);
        }


        [HttpPost("registrar")]
        public async Task<ActionResult<Limpeza>> RegistrarLimpeza([FromBody] Limpeza limpeza)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se o quarto existe
                var quarto = await _context.Quartos.FindAsync(limpeza.QuartosId);
                if (quarto == null)
                {
                    return BadRequest("O quarto especificado não existe.");
                }

                // Verificar se o funcionário existe
                var funcionario = await _context.Funcionarios.FindAsync(limpeza.FuncionariosId);
                if (funcionario == null)
                {
                    return BadRequest("O funcionário especificado não existe.");
                }

                // Adicionar a limpeza ao contexto
                _context.Limpezas.Add(limpeza);
                await _context.SaveChangesAsync();

                // Retornar os detalhes da limpeza registrada
                return CreatedAtAction(nameof(ObterLimpeza), new { id = limpeza.Id }, limpeza);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao registrar a limpeza: {ex.Message}");
            }
        }

    }
}
