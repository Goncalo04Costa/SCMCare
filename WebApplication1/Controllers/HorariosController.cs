using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.RegrasNegocio;


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
            var horario = await _context.Horarios.FindAsync(id); // !!! verificar se funciona

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
            var regrasHorario = new RegrasHorario(_context);
            var horarioValido = await regrasHorario.HorarioEValido(horario);

            if (!horarioValido)
            {
                return BadRequest("Já existe um horário agendado para o funcionário neste dia e turno");
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

        [HttpGet("ObterHorariosPorCat/{tipoFuncionarioId}")]
        public async Task<ActionResult<IEnumerable<Horario>>> ObterHorariosPorCat(int tipoFuncionarioId)
        {

            var funcionarios = await _context.Funcionarios
                .Where(f => f.TiposFuncionarioId == tipoFuncionarioId)
                .ToListAsync();

            if (funcionarios == null || !funcionarios.Any())
            {
                return NotFound($"Não foram encontrados funcionários com o tipo de funcionário ID {tipoFuncionarioId}");
            }


            var funcionariosIds = funcionarios.Select(f => f.Id).ToList();


            var horarios = await _context.Horarios
                .Where(h => funcionariosIds.Contains(h.FuncionariosId))
                .ToListAsync();

            if (horarios == null || !horarios.Any())
            {
                return NotFound($"Não foram encontrados horários para os funcionários com o tipo de funcionário ID {tipoFuncionarioId}");
            }

            return Ok(horarios);
        }

        [HttpGet("ObterHorariosPorFuncionario/{funcionarioId}")]
        public async Task<ActionResult<IEnumerable<Horario>>> ObterHorariosPorFuncionario(int funcionarioId)
        {
            var horarios = await _context.Horarios.Where(h => h.FuncionariosId == funcionarioId).ToListAsync();

            if (horarios == null || !horarios.Any())
            {
                return NotFound($"Não foram encontrados horários para o funcionário com o ID {funcionarioId}");
            }

            return Ok(horarios);
        }

        [HttpGet("ObterHorariosTodosFuncionarios")]
        //[RegrasHorario.AutorizacaoHorarios] 
        public async Task<ActionResult<IEnumerable<Horario>>> ObterHorariosTodosFuncionarios()
        {
            var tipoFuncionarioUsuario = 1; 
            if (tipoFuncionarioUsuario != 1 && tipoFuncionarioUsuario != 2 && tipoFuncionarioUsuario != 3)
            {
                return Forbid(); 
            }
            var horarios = await _context.Horarios.ToListAsync();

            if (horarios == null || !horarios.Any())
            {
                return NotFound("Não foram encontrados horários para nenhum funcionário");
            }
                return Ok(horarios);
        }


    }
}
