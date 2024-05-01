using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConsultasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> ObterTodasConsultas()
        {
            var consultas = await _context.Consultas.ToListAsync();
            return Ok(consultas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> ObterConsulta(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
            {
                return NotFound();
            }

            return Ok(consulta);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> RegistrarConsulta(int idConsulta, int idFuncionario, int idResponsavel, DateTime dataConsulta)
        {
            var consulta = await _context.Consultas.FindAsync(idConsulta);
            if (consulta == null)
            {
                return NotFound($"Consulta com o ID {idConsulta} não encontrada");
            }

            var funcionario = await _context.Funcionarios.FindAsync(idFuncionario);
            if (funcionario == null)
            {
                return NotFound($"Funcionário com o ID {idFuncionario} não encontrado");
            }

            var responsavel = await _context.Responsaveis.FindAsync(idResponsavel);
            if (responsavel == null)
            {
                return NotFound($"Responsável com o ID {idResponsavel} não encontrado");
            }

           
            var canAttendResult = await CanAttendConsulta(idResponsavel, dataConsulta);
            if (canAttendResult is OkObjectResult)
            {
                return Ok("O responsável pode comparecer à consulta. Não é necessário nomear um funcionário.");
            }
            else if (canAttendResult is OkObjectResult)
            {
             
                funcionario.FuncionarioID = idFuncionario;

                await _context.SaveChangesAsync();

                return Ok($"Funcionário com o ID {idFuncionario} nomeado para a consulta com o ID {idConsulta}.");
            }
            else
            {
                return StatusCode(500, "Erro interno do servidor ao verificar a disponibilidade do responsável.");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarConsulta(int id, [FromBody] Consulta novaConsulta)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
            {
                return NotFound($"Não foi possível encontrar a consulta com o ID {id}");
            }

            consulta.Data = novaConsulta.Data;
            consulta.Descricao = novaConsulta.Descricao;
            consulta.HospitaisId = novaConsulta.HospitaisId;
            consulta.UtentesId = novaConsulta.UtentesId;
            consulta.FuncionariosId = novaConsulta.FuncionariosId;
            consulta.ResponsaveisId = novaConsulta.ResponsaveisId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Consulta atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverConsulta(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
            {
                return NotFound($"Consulta com o ID {id} não encontrada");
            }

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            return Ok($"Consulta com o ID {id} removida com sucesso");
        }


        [HttpGet("CanAttendConsulta")]
        public async Task<IActionResult> CanAttendConsulta(int responsavelId, DateTime consultaData)
        {
          
            var outraConsulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.ResponsaveisId == responsavelId && c.Data == consultaData.Date);

            if (outraConsulta != null)
            {
                return Ok("O responsável não pode comparecer à consulta.");
            }
            else
            {
                return Ok("O responsável pode comparecer à consulta");
            }
        }


      

    }
}
