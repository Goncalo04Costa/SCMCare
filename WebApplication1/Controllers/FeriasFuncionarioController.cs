using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FeriasFuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeriasFuncionarioController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeriasFuncionario>>> ObterTodasFeriasFuncionario()
        {
            var feriasfuncionarios = await _context.FeriasFuncionario.ToListAsync();
            return Ok(feriasfuncionarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeriasFuncionario>> ObterFeriaFuncionario(int id)
        {
            var feriasfuncionario = await _context.FeriasFuncionario.FindAsync(id);

            if (feriasfuncionario = null)
            {
                return NotFound();
            }
            return Ok(feriasfuncionario);
        }

        [HttpPost]
        public async Task<ActionResult<FeriasFuncionario>> InserirFeriasFuncionario([FromBody] FeriasFuncionario feriaFuncionario)
        {
            if (feriaFuncionario == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.FeriasFuncionario.Add(feriaFuncionario);
            await _context.SaveChangesAsync();

            return Ok("FeriasFuncionario adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaFeriasFuncionario(int id, [FromBody] FeriasFuncionario novaFeriaFuncionario)
        {
            var feriaFuncionario = await _context.FeriasFuncionario.FindAsync(id);

            if (feriaFuncionario == null)
            {
                return NotFound($"Não foi possível encontrar a feriaFuncionario com o ID {id}");
            }

            feriaFuncionario.FuncionariosId = novaFeriaFuncionario.FuncionariosId;
            feriaFuncionario.FuncionariosIdValida = novaFeriaFuncionario.FuncionariosIdValida;
            feriaFuncionario.Dia = novaFeriaFuncionario.Dia;
            feriaFuncionario.Estado = novaFeriaFuncionario.Estado;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizada a feriaFuncionario com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFeriasFuncionario(int id)
        {
            var feriaFuncionario = await _context.FeriasFuncionario.FindAsync(id);

            if (feriaFuncionario == null)
            {
                return NotFound($"Não foi possível encontrar a feriaFuncionario com o ID {id}");
            }

            _context.FeriasFuncionario.Remove(feriaFuncionario);
            await _context.SaveChangesAsync();

            return Ok($"Foi removida a feriaFuncionario com o ID {id}");
        }

    }
}