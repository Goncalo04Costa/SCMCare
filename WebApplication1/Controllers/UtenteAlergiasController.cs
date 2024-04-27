using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UtenteAlergiasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UtenteAlergiasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UtenteAlergia>>> ObterTodosUtenteAlergias(
            int? idUtenteMin = null, int? idUtenteMax = null,
            int? idAlergiaMin = null, int? idAlergiaMax = null)
        {
            IQueryable<UtenteAlergia> query = _context.UtentesAlergias;

            if (idUtenteMin.HasValue)
            {
                query = query.Where(d => d.UtentesId >= idUtenteMin.Value);
            }

            if (idUtenteMax.HasValue)
            {
                query = query.Where(d => d.UtentesId <= idUtenteMax.Value);
            }

            if (idAlergiaMin.HasValue)
            {
                query = query.Where(d => d.TiposAlergiaId >= idAlergiaMin.Value);
            }

            if (idAlergiaMax.HasValue)
            {
                query = query.Where(d => d.TiposAlergiaId <= idAlergiaMax.Value);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Utente>> ObterUtenteAlergias(int id)
        {
            IQueryable<UtenteAlergia> query = _context.UtentesAlergias;

            query = query.Where(d => d.UtentesId == id);

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpPost]
        public async Task<ActionResult<UtenteAlergia>> InserirUtenteAlergia([FromBody] UtenteAlergia utAl)
        {
            if (utAl == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.UtentesAlergias.Add(utAl);
            await _context.SaveChangesAsync();

            return Ok("Alergia adicionada ao utente com sucesso");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UtenteAlergia>> RemoveUtenteAlergia([FromBody] UtenteAlergia utAl)
        {
            if (utAl == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.UtentesAlergias.Remove(utAl);
            await _context.SaveChangesAsync();

            return Ok("Alergia removida do utente com sucesso");
        }
    }
}
