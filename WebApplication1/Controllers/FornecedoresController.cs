using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FornecedoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> ObterTodosFornecedores(
            int? idMin = null, int? idMax = null,
            string? nomeMin = null, string? nomeMax = null)
        {
            IQueryable<Fornecedor> query = _context.Fornecedores;

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

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> ObterFornecedor(int id)
        {
            IQueryable<Fornecedor> query = _context.Fornecedores;
            query = query.Where(d => d.Id == id);

            var fornecedorDetalhes = await (
                from fornecedor in query
                select new
                {
                    Id = fornecedor.Id,
                    Nome = fornecedor.Nome,
                    Contactos = _context.ContactosFornecedores
                        .Where(cf => cf.FornecedoresId == fornecedor.Id)
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

            return Ok(fornecedorDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Fornecedor>> InserirFornecedor([FromBody] Fornecedor fornecedor)
        {
            if (fornecedor == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            return Ok("Fornecedor adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarFornecedor(int id, [FromBody] Fornecedor novoFornecedor)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if (fornecedor == null)
            {
                return NotFound($"Não foi possível encontrar o fornecedor com o ID {id}");
            }

            fornecedor.Nome = novoFornecedor.Nome;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Fornecedor atualizado com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverFornecedor(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if (fornecedor == null)
            {
                return NotFound($"Fornecedor com o ID {id} não encontrado");
            }

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return Ok($"Fornecedor com o ID {id} removido com sucesso");
        }
    }
}
