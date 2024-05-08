using iText.Kernel.Pdf.Canvas.Wmf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CamasController(AppDbContext context)
        {
            _context = context;
        }


        // Metodo para obter todas as camas com filtros especificos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cama>>> ObterTodasCamas(
            string? idMin = null, string? idMax = null,
            int? utenteId = null,
            int? quartoId = null)
        {
            IQueryable<Cama> query = _context.Camas;

            if (!string.IsNullOrEmpty(idMin))
            {
                query = query.Where(d => d.Id.CompareTo(idMin) >= 0);
            }

            if (!string.IsNullOrEmpty(idMax))
            {
                query = query.Where(d => d.Id.CompareTo(idMax + "ZZZ") <= 0);
            }

            if (utenteId.HasValue)
            {
                query = query.Where(d => d.UtentesId == utenteId.Value);
            }

            if (quartoId.HasValue)
            {
                query = query.Where(d => d.QuartosId == quartoId.Value);
            }


            var camasDetalhes = await (
                from cama in query
                join utente in _context.Utentes on cama.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join quarto in _context.Quartos on cama.QuartosId equals quarto.Id into qG
                from quarto in qG.DefaultIfEmpty()
                select new
                {
                    Id = cama.Id,
                    UtentesId = cama.UtentesId,
                    Utentes = utente.Nome,
                    QuartoId = cama.QuartosId,
                    Quarto = quarto.Numero
                }
            ).ToListAsync();

            return Ok(camasDetalhes);
        }


        //Metodo para obter cama
        [HttpGet("{idUtente}")]
        public async Task<ActionResult<Cama>> ObterCama(int idUtente)
        {
            IQueryable<Cama> query = _context.Camas;
            query = query.Where(d => d.UtentesId == idUtente);


            var camaDetalhes = await (
                from cama in query
                join utente in _context.Utentes on cama.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join quarto in _context.Quartos on cama.QuartosId equals quarto.Id into qG
                from quarto in qG.DefaultIfEmpty()
                select new
                {
                    Id = cama.Id,
                    UtentesId = cama.UtentesId,
                    Utentes = utente.Nome,
                    QuartoId = cama.QuartosId,
                    Quarto = quarto.Numero
                }
            ).ToListAsync();

            return Ok(camaDetalhes);
        }


        // Metodo para inserir uma nova cama
        [HttpPost]
        public async Task<ActionResult<Cama>> InserirCama([FromBody] Cama cama)
        {
            if (cama == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Camas.Add(cama);
            await _context.SaveChangesAsync();

            return Ok("Cama adicionada com sucesso");
        }


        // Metodo para atualizar dados de uma cama
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCama(int id, [FromBody] Cama novaCama)
        {
            var cama = await _context.Camas.FindAsync(id);

            if (cama == null)
            {
                return NotFound($"Não foi possível encontrar a cama com o ID {id}");
            }

            cama.UtentesId = novaCama.UtentesId;
            cama.QuartosId = novaCama.QuartosId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Cama atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //Metodo para remover uma cama
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverCama(int id)
        {
            var cama = await _context.Camas.FindAsync(id);

            if (cama == null)
            {
                return NotFound($"Cama com o ID {id} não encontrada");
            }

            _context.Camas.Remove(cama);
            await _context.SaveChangesAsync();

            return Ok($"Cama com o ID {id} removida com sucesso");
        }
    }
}
