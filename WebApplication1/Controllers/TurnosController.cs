using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TurnosController : Controller
    {
        private readonly AppDbContext _context;

        public TurnosController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turno>>> ObterTodosTurnos(
            int? idMin = null, int? idMax = null,
            TimeOnly? horaIniMin = null, TimeOnly? horaIniMax = null,
            TimeOnly? horaFimMin = null, TimeOnly? horaFimMax = null,
            bool ativo0 = false, bool ativo1 = false)
        {
            IQueryable<Turno> query = _context.Turnos;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (horaIniMin.HasValue)
            {
                query = query.Where(d => d.HoraInicio >= horaIniMin.Value);
            }

            if (horaIniMax.HasValue)
            {
                query = query.Where(d => d.HoraInicio <= horaIniMax.Value);
            }

            if (horaFimMin.HasValue)
            {
                query = query.Where(d => d.HoraFim >= horaFimMin.Value);
            }

            if (horaFimMax.HasValue)
            {
                query = query.Where(d => d.HoraFim <= horaFimMax.Value);
            }

            if (ativo0 && !ativo1)
            {
                query = query.Where(d => !d.Ativo); // Mostra turnos não ativos
            }

            else if (!ativo0 && ativo1)
            {
                query = query.Where(d => d.Ativo); // Mostra turnos ativos
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Turno>> ObterTurno(int id)
        {
            var dado = await _context.Turnos.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<Turno>> InserirTurno([FromBody] Turno turno)
        {
            if (turno == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return Ok("Turno adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTurno(int id, [FromBody] Turno novoTurno)
        {
            var turno = await _context.Turnos.FirstOrDefaultAsync(d => d.Id == id);
            if (turno == null)
            {
                return NotFound($"Não foi possível encontrar o turno com o ID {id}");
            }

            turno.HoraInicio = novoTurno.HoraInicio;
            turno.HoraFim = novoTurno.HoraFim;
            turno.Ativo = novoTurno.Ativo;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o turno com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTurno(int id)
        {
            var turno = await _context.Turnos.FirstOrDefaultAsync(d => d.Id == id);
            if (turno == null)
            {
                return NotFound($"Não foi possível encontrar o turno com o ID {id}");
            }

            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o turno com o ID {id}");
        }
    }
}
