using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContactosFornecedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactosFornecedoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactoFornecedor>>> ObterTodosContactoFornecedor()
        {
            var contactoFornecedor = await _context.ContactosFornecedores.ToListAsync();
            return Ok(contactoFornecedor);
        }

        [HttpGet("{FornecedoresId}/{TipoContactoId}")]
        public async Task<ActionResult<ContactoFornecedor>> ObterContactoFornecedor(int FornecedoresId, int TipoContactoId)
        {
            var contactoFornecedor = await _context.ContactosFornecedores.FirstOrDefaultAsync(a => a.FornecedoresId == FornecedoresId && a.TipoContactoId == TipoContactoId);

            if (contactoFornecedor = null)
            {
                return NotFound();
            }
            return Ok(contactoFornecedor);
        }

        [HttpPost]
        public async Task<ActionResult<ContactoFornecedor>> InserirContactoFornecedor([FromBody] ContactoFornecedor contactoFornecedor)
        {
            if (contactoFornecedor == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.ContactosFornecedores.Add(contactoFornecedor);
            await _context.SaveChangesAsync();

            return Ok("contactoFornecedor adicionado com sucesso");
        }

        [HttpPut("{FornecedoresId}/{TipoContactoId}")]
        public async Task<IActionResult> AtualizaContactoFornecedor(int FornecedoresId, int TipoContactoId, [FromBody] ContactoFornecedor novoContactoFornecedor)
        {
            var contactoFornecedor = await _context.ContactosFornecedores.FirstOrDefaultAsync(a => a.FornecedoresId == FornecedoresId && a.TipoContactoId == TipoContactoId);

            if (contactoFornecedor == null)
            {
                return NotFound($"Não foi possível encontrar o contactoFornecedor com o fornecedor ID {FornecedoresId} e tipo de contacto ID {TipoContactoId}");
            }

            contactoFornecedor.Valor = novoContactoFornecedor.Valor;
            
            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o contactoFornecedor com o fornecedor ID {FornecedoresId} e tipo de contacto ID {TipoContactoId}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{FornecedoresId}/{TipoContactoId}")]
        public async Task<IActionResult> RemoveContactoFornecedor(int FornecedoresId, int TipoContactoId)
        {
            var contactoFornecedor = await _context.ContactosFornecedores.FirstOrDefaultAsync(a => a.FornecedoresId == FornecedoresId && a.TipoContactoId == TipoContactoId);

            if (contactoFornecedor == null)
            {
                return NotFound($"Não foi possível encontrar o contactoFornecedor com o fornecedor ID {FornecedoresId} e tipo de contacto ID {TipoContactoId}");
            }

            _context.ContactosFornecedores.Remove(contactoFornecedor);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o contactoFornecedor com o fornecedor ID {FornecedoresId} e tipo de contacto ID {TipoContactoId}");
        }
    }
}