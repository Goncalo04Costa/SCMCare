using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsaveisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResponsaveisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Responsavel>>> ObterTodosResponsaveis(
            int? idMin = null, int? idMax = null,
            string? nomeMin = null, string? nomeMax = null,
            int? utenteId = null)
        {
            IQueryable<Responsavel> query = _context.Responsaveis;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMin) >= 0);
            }

            if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMax + "ZZZ") <= 0);
            }

            if (utenteId.HasValue)
            {
                query = query.Where(d => d.UtentesId == utenteId.Value);
            }
            var responsavelDetalhes = await (
                from responsavel in query
                join utente in _context.Utentes on responsavel.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                select new
                {
                    Id = responsavel.Id,
                    Nome = responsavel.Nome,
                    UtentesId = responsavel.UtentesId,
                    Utente = utente.Nome,
                    Morada = responsavel.Morada
                }
            ).ToListAsync();

            return Ok(responsavelDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Responsavel>> ObterResponsavel(int id)
        {
            IQueryable<Responsavel> query = _context.Responsaveis;
            query = query.Where(d => d.Id == id); 
            
            var responsavelDetalhes = await (
                from responsavel in query
                join utente in _context.Utentes on responsavel.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                select new
                {
                    Id = responsavel.Id,
                    Nome = responsavel.Nome,
                    UtentesId = responsavel.UtentesId,
                    Utente = utente.Nome,
                    Morada = responsavel.Morada,
                    Contactos = _context.ContactosResponsaveis
                        .Where(cf => cf.ResponsaveisId == responsavel.Id)
                        .Join(
                            _context.TiposContacto,
                            cf => cf.TipoContactoId,
                            tc => tc.Id,
                            (cf, tc) => new
                            {
                                TipoContactoId = tc.Id,
                                TipoContacto = tc.Descricao,
                                Valor = cf.Valor
                            }
                        )
                        .ToList()
                }
            ).ToListAsync();

            return Ok(responsavelDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Responsavel>> InserirResponsavel([FromBody] Responsavel responsavel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Responsaveis.Add(responsavel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterResponsavel), new { id = responsavel.Id }, responsavel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarResponsavel(int id, [FromBody] Responsavel responsavel)
        {
            if (id != responsavel.Id)
            {
                return BadRequest();
            }

            _context.Entry(responsavel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponsavelExists(id))
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
        public async Task<IActionResult> RemoverResponsavel(int id)
        {
            var responsavel = await _context.Responsaveis.FindAsync(id);
            if (responsavel == null)
            {
                return NotFound();
            }

            _context.Responsaveis.Remove(responsavel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResponsavelExists(int id)
        {
            return _context.Responsaveis.Any(e => e.Id == id);
        }
    }
}
