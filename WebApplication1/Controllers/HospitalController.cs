using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HospitaisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hospital>>> ObterTodosHospitais()
        {
            var hospitais = await _context.Hospitais.ToListAsync();
            return Ok(hospitais);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hospital>> ObterHospital(int id)
        {
            var hospital = await _context.Hospitais.FindAsync(id);

            if (hospital == null)
            {
                return NotFound();
            }

            return Ok(hospital);
        }

        [HttpPost]
        public async Task<ActionResult<Hospital>> InserirHospital([FromBody] Hospital hospital)
        {
            if (hospital == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Hospitais.Add(hospital);
            await _context.SaveChangesAsync();

            return Ok("Hospital adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarHospital(int id, [FromBody] Hospital novoHospital)
        {
            var hospital = await _context.Hospitais.FindAsync(id);

            if (hospital == null)
            {
                return NotFound($"Não foi possível encontrar o hospital com o ID {id}");
            }

            hospital.Nome = novoHospital.Nome;
            hospital.Morada = novoHospital.Morada;
            hospital.Ativo = novoHospital.Ativo;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Hospital atualizado com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverHospital(int id)
        {
            var hospital = await _context.Hospitais.FindAsync(id);

            if (hospital == null)
            {
                return NotFound($"Hospital com o ID {id} não encontrado");
            }

            _context.Hospitais.Remove(hospital);
            await _context.SaveChangesAsync();

            return Ok($"Hospital com o ID {id} removido com sucesso");
        }
    }
}
