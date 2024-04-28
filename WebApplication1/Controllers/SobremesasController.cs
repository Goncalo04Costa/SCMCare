using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SobremesasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SobremesasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sobremesa>>> ObterTodasSobremesas(
            int? idMin = null, int? idMax = null, 
            string? nomeMin = null, string? nomeMax = null, 
            string? descMin = null, string? descMax = null, 
            bool tipo0 = false, bool tipo1 = false,
            bool ativo0 = false, bool ativo1 = false)
        {
            IQueryable<Sobremesa> query = _context.Sobremesas;

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
                query = query.Where(d => d.Nome.CompareTo(nomeMax+"ZZZ") <= 0);
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
                query = query.Where(d => !d.Tipo); // Mostra sobremesas com tipo 0
            }

            else if (!tipo0 && tipo1)
            {
                query = query.Where(d => d.Tipo); // Mostra sobremesas com tipo 1
            }

            if (ativo0 && !ativo1)
            {
                query = query.Where(d => !d.Ativo);
            }

            else if (!ativo0 && ativo1)
            {
                query = query.Where(d => d.Ativo);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sobremesa>> ObterSobremesa(int id)
        {
            var dado = await _context.Sobremesas.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpPost]
        public async Task<ActionResult<Sobremesa>> InserirSobremesa([FromBody] Sobremesa sobremesa)
        {
            if (sobremesa == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Sobremesas.Add(sobremesa);
            await _context.SaveChangesAsync();

            return Ok("Sobremesa adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaSobremesa(int id, [FromBody] Sobremesa novaSobremesa)
        {
            var sobremesa = await _context.Sobremesas.FirstOrDefaultAsync(d => d.Id == id);
            if (sobremesa == null)
            {
                return NotFound($"Não foi possível encontrar a sobremesa com o ID {id}");
            }

            sobremesa.Nome = novaSobremesa.Nome;
            sobremesa.Descricao = novaSobremesa.Descricao;
            sobremesa.Tipo = novaSobremesa.Tipo;
            sobremesa.Ativo = novaSobremesa.Ativo;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizada a sobremesa com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSobremesa(int id)
        {
            var sobremesa = await _context.Sobremesas.FirstOrDefaultAsync(d => d.Id == id);
            if (sobremesa == null)
            {
                return NotFound($"Não foi possível encontrar a sobremesa com o ID {id}");
            }

            _context.Sobremesas.Remove(sobremesa);
            await _context.SaveChangesAsync();

            return Ok($"Foi removida a sobremesa com o ID {id}");
        }
    }
}
