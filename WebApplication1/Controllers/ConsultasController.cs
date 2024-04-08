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
        public async Task<ActionResult<Consulta>> InserirConsulta([FromBody] Consulta consulta)
        {
            if (consulta == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            return Ok("Consulta adicionada com sucesso");
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
            var canAttend = true;

            if (canAttend)
            {
                return Ok("O responsável pode comparecer à consulta");
            }
            else
            {
                return Ok("O responsável não pode comparecer à consulta");
            }
        }
    }
}
