using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SopasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SopasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sopa>>> ObterTodasSopas(
            int? idMin = null, int? idMax = null,
            string nomeMin = null, string nomeMax = null,
            string descMin = null, string descMax = null,
            bool tipo0 = false, bool tipo1 = false)
        {
            IQueryable<Sopa> query = _context.Sopas;

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

            if (!string.IsNullOrEmpty(descMin))
            {
                query = query.Where(d => d.Descricao.CompareTo(descMin) >= 0);
            }

            if (!string.IsNullOrEmpty(descMax))
            {
                query = query.Where(d => d.Descricao.CompareTo(descMax + "ZZZ") <= 0);
            }

            if (tipo0 && !tipo1)
            {
                query = query.Where(d => !d.Tipo); // Mostra sopas com tipo 0
            }
            else if (!tipo0 && tipo1)
            {
                query = query.Where(d => d.Tipo); // Mostra sopas com tipo 1
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sopa>> ObterSopa(int id)
        {
            var dado = await _context.Sopas.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<Sopa>> InserirSopa([FromBody] Sopa sopa)
        {
            if (sopa == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Sopas.Add(sopa);
            await _context.SaveChangesAsync();

            return Ok("Sopa adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaSopa(int id, [FromBody] Sopa novaSopa)
        {
            var sopa = await _context.Sopas.FirstOrDefaultAsync(d => d.Id == id);
            if (sopa == null)
            {
                return NotFound($"Não foi possível encontrar a sopa com o ID {id}");
            }

            sopa.Nome = novaSopa.Nome;
            sopa.Descricao = novaSopa.Descricao;
            sopa.Tipo = novaSopa.Tipo;
            sopa.Ativo = novaSopa.Ativo;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizada a sopa com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSopa(int id)
        {
            var sopa = await _context.Sopas.FirstOrDefaultAsync(d => d.Id == id);
            if (sopa == null)
            {
                return NotFound($"Não foi possível encontrar a sopa com o ID {id}");
            }

            _context.Sopas.Remove(sopa);
            await _context.SaveChangesAsync();

            return Ok($"Foi removida a sopa com o ID {id}");
        }
    }
}
