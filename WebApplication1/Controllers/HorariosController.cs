using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HorariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Horario>>> ObterTodosHorarios()
        {
            var horarios = await _context.Horarios.ToListAsync();
            return Ok(horarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Horario>> ObterHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);

            if (horario == null)
            {
                return NotFound();
            }

            return Ok(horario);
        }

        [HttpPost]
        public async Task<ActionResult<Horario>> InserirHorario([FromBody] Horario horario)
        {
            if (horario == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Horarios.Add(horario);
            await _context.SaveChangesAsync();

            return Ok("Horário adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarHorario(int id, [FromBody] Horario novoHorario)
        {
            var horario = await _context.Horarios.FindAsync(id);

            if (horario == null)
            {
                return NotFound($"Não foi possível encontrar o horário com o ID {id}");
            }

            horario.FuncionariosId = novoHorario.FuncionariosId;
            horario.TurnosId = novoHorario.TurnosId;
            horario.Dia = novoHorario.Dia;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Horário atualizado com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);

            if (horario == null)
            {
                return NotFound($"Horário com o ID {id} não encontrado");
            }

            _context.Horarios.Remove(horario);
            await _context.SaveChangesAsync();

            return Ok($"Horário com o ID {id} removido com sucesso");
        }
    }
}
