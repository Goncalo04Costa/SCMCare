using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



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
        public async Task<ActionResult<IEnumerable<Horario>>> ObterTodosHorarios(
            int? funcionarioIdMin = null, int? funcionarioIdMax = null,
            int? turnoIdMin = null, int? turnoIdMax = null,
            DateTime? diaMin = null, DateTime? diaMax = null)
        {
            IQueryable<Horario> query = _context.Horarios;

            if (funcionarioIdMin.HasValue)
            {
                query = query.Where(d => d.FuncionariosId >= funcionarioIdMin.Value);
            }

            if (funcionarioIdMax.HasValue)
            {
                query = query.Where(d => d.FuncionariosId <= funcionarioIdMax.Value);
            }

            if (turnoIdMin.HasValue)
            {
                query = query.Where(d => d.TurnosId >= turnoIdMin.Value);
            }

            if (turnoIdMin.HasValue)
            {
                query = query.Where(d => d.TurnosId <= turnoIdMin.Value);
            }

            if (diaMin.HasValue)
            {
                query = query.Where(d => d.Dia >= diaMin.Value);
            }

            if (diaMax.HasValue)
            {
                query = query.Where(d => d.Dia <= diaMax.Value);
            }


            var horariosDetalhes = await (
                from horario in query
                join funcionario in _context.Funcionarios on horario.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join turno in _context.Turnos on horario.TurnosId equals turno.Id into tG
                from turno in tG.DefaultIfEmpty()
                select new
                {
                    FuncionariosId = horario.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    TurnoId = horario.TurnosId,
                    TurnoInicio = turno.HoraFim,
                    TurnoFim = turno.HoraFim,
                    Dia = horario.Dia
                }
            ).ToListAsync();

            return Ok(horariosDetalhes);
        }


        [HttpGet("{funcionarioId}/{turnoId}/{dia}")]
        public async Task<ActionResult<Horario>> ObterHorario(int funcionarioId, int turnoId, DateTime dia)
        {
            IQueryable<Horario> query = _context.Horarios;
            query = query.Where(d => d.FuncionariosId == funcionarioId && d.TurnosId == turnoId && d.Dia == dia);

            var horariosDetalhes = await (
                from horario in query
                join funcionario in _context.Funcionarios on horario.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join turno in _context.Turnos on horario.TurnosId equals turno.Id into tG
                from turno in tG.DefaultIfEmpty()
                select new
                {
                    FuncionariosId = horario.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    TurnoId = horario.TurnosId,
                    TurnoInicio = turno.HoraFim,
                    TurnoFim = turno.HoraFim,
                    Dia = horario.Dia
                }
            ).ToListAsync();

            return Ok(horariosDetalhes);
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


        [HttpPut("{funcionarioId}/{turnoId}/{dia}")]
        public async Task<IActionResult> AtualizarHorario(int funcionarioId, int turnoId, DateTime dia, [FromBody] Horario novoHorario)
        {
            var horario = await _context.Horarios.FirstOrDefaultAsync(d => d.FuncionariosId == funcionarioId && d.TurnosId == turnoId && d.Dia == dia);

            if (horario == null)
            {
                return NotFound($"Não foi possível encontrar o horário do funcionário com o ID {funcionarioId}, para o turno {turnoId} no dia {dia}");
            }

            horario.FuncionariosId = novoHorario.FuncionariosId;
            horario.TurnosId = novoHorario.TurnosId;
            horario.Dia = novoHorario.Dia;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Horário atualizado com sucesso");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{funcionarioId}/{turnoId}/{dia}")]
        public async Task<IActionResult> RemoverHorario(int funcionarioId, int turnoId, DateTime dia)
        {
            var horario = await _context.Horarios.FirstOrDefaultAsync(d => d.FuncionariosId == funcionarioId && d.TurnosId == turnoId && d.Dia == dia);

            if (horario == null)
            {
                return NotFound($"Não foi possível encontrar o horário do funcionário com o ID {funcionarioId}, para o turno {turnoId} no dia {dia}");
            }

            _context.Horarios.Remove(horario);
            await _context.SaveChangesAsync();

            return Ok($"Horário removido com sucesso");
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


            var funcionariosIds = funcionarios.Select(f => f.FuncionarioID).ToList();


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


        // !!! Porquê v v v

        //[HttpGet("ObterHorariosTodosFuncionarios")]
        ////[RegrasHorario.AutorizacaoHorarios] 
        //public async Task<ActionResult<IEnumerable<Horario>>> ObterHorariosTodosFuncionarios()
        //{
        //    var tipoFuncionarioUsuario = 1; 
        //    if (tipoFuncionarioUsuario != 1 && tipoFuncionarioUsuario != 2 && tipoFuncionarioUsuario != 3)
        //    {
        //        return Forbid(); 
        //    }
        //    var horarios = await _context.Horarios.ToListAsync();

        //    if (horarios == null || !horarios.Any())
        //    {
        //        return NotFound("Não foram encontrados horários para nenhum funcionário");
        //    }
        //        return Ok(horarios);
        //}


    }
}
