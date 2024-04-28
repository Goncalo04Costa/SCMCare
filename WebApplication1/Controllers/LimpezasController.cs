using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimpezasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LimpezasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Limpeza>>> ObterTodasLimpezas(
            int? idMin = null, int? idMax = null,
            DateTime? dataMin = null, DateTime? dataMax = null,
            int? quartosIdMin = null, int? quartosIdMax = null,
            int? funcIdMin = null, int? funcIdMax = null)
        {
            IQueryable<Limpeza> query = _context.Limpezas;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (dataMin.HasValue)
            {
                query = query.Where(d => d.Data <= dataMin.Value);
            }

            if (dataMax.HasValue)
            {
                query = query.Where(d => d.Data >= dataMax.Value);
            }

            if (quartosIdMin.HasValue)
            {
                query = query.Where(d => d.QuartosId >= quartosIdMin.Value);
            }

            if (quartosIdMax.HasValue)
            {
                query = query.Where(d => d.QuartosId <= quartosIdMax.Value);
            }

            if (funcIdMin.HasValue)
            {
                query = query.Where(d => d.FuncionariosId >= funcIdMin.Value);
            }

            if (funcIdMax.HasValue)
            {
                query = query.Where(d => d.FuncionariosId <= funcIdMax.Value);
            }


            var limpezaDetalhes = await (
                from limpeza in query
                join quarto in _context.Quartos on limpeza.QuartosId equals quarto.Id into sQ
                from quarto in sQ.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on limpeza.FuncionariosId equals funcionario.Id into fQ
                from funcionario in fQ.DefaultIfEmpty()
                select new
                {
                    Id = limpeza.Id,
                    Data = limpeza.Data,
                    QuartosId = limpeza.QuartosId,
                    Quarto = quarto.Numero,
                    FuncionariosId = limpeza.FuncionariosId,
                    Funcionario = funcionario.Nome
                }
            ).ToListAsync();

            return Ok(limpezaDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Limpeza>> ObterLimpeza(int id)
        {
            IQueryable<Limpeza> query = _context.Limpezas;
            query = query.Where(d => d.Id == id);

            var limpezaDetalhes = await (
                from limpeza in query
                join quarto in _context.Quartos on limpeza.QuartosId equals quarto.Id into sQ
                from quarto in sQ.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on limpeza.FuncionariosId equals funcionario.Id into fQ
                from funcionario in fQ.DefaultIfEmpty()
                select new
                {
                    Id = limpeza.Id,
                    Data = limpeza.Data,
                    QuartosId = limpeza.QuartosId,
                    Quarto = quarto.Numero,
                    FuncionariosId = limpeza.FuncionariosId,
                    Funcionario = funcionario.Nome
                }
            ).ToListAsync();

            return Ok(limpezaDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Limpeza>> InserirLimpeza([FromBody] Limpeza limpeza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Limpezas.Add(limpeza);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterLimpeza), new { id = limpeza.Id }, limpeza);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarLimpeza(int id, [FromBody] Limpeza limpeza)
        {
            if (id != limpeza.Id)
            {
                return BadRequest();
            }

            _context.Entry(limpeza).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LimpezaExists(id))
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
        public async Task<IActionResult> RemoverLimpeza(int id)
        {
            var limpeza = await _context.Limpezas.FindAsync(id);
            if (limpeza == null)
            {
                return NotFound();
            }

            _context.Limpezas.Remove(limpeza);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LimpezaExists(int id)
        {
            return _context.Limpezas.Any(e => e.Id == id);
        }


        [HttpPost("registrar")]
        public async Task<ActionResult<Limpeza>> RegistrarLimpeza([FromBody] Limpeza limpeza)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se o quarto existe
                var quarto = await _context.Quartos.FindAsync(limpeza.QuartosId);
                if (quarto == null)
                {
                    return BadRequest("O quarto especificado não existe.");
                }

                // Verificar se o funcionário existe
                var funcionario = await _context.Funcionarios.FindAsync(limpeza.FuncionariosId);
                if (funcionario == null)
                {
                    return BadRequest("O funcionário especificado não existe.");
                }

                // Adicionar a limpeza ao contexto
                _context.Limpezas.Add(limpeza);
                await _context.SaveChangesAsync();

                // Retornar os detalhes da limpeza registrada
                return CreatedAtAction(nameof(ObterLimpeza), new { id = limpeza.Id }, limpeza);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao registrar a limpeza: {ex.Message}");
            }
        }

    }
}
