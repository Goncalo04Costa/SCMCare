using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContactosFuncionariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactosFuncionariosController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactoFuncionario>>> ObterTodosContactoFuncionario()
        {
            var contactoFuncionario = await _context.ContactosFuncionarios.ToListAsync();
            return Ok(contactoFuncionario);
        }

        [HttpGet("{FuncionariosId}/{TipoContactoId}")]
        public async Task<ActionResult<ContactoFuncionario>> ObterContactoFuncionario(int FuncionariosId, int TipoContactoId)
        {
            var contactoFuncionario = await _context.ContactosFuncionarios.FirstOrDefaultAsync(a => a.FuncionariosId == FuncionariosId && a.TipoContactoId == TipoContactoId);

            if (contactoFuncionario == null)
            {
                return NotFound();
            }
            return Ok(contactoFuncionario);
        }

        [HttpPost]
        public async Task<ActionResult<ContactoFuncionario>> InserirContactoFuncionario([FromBody] ContactoFuncionario contactoFuncionario)
        {
            if (contactoFuncionario == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.ContactosFuncionarios.Add(contactoFuncionario);
            await _context.SaveChangesAsync();

            return Ok("contactoFuncionario adicionado com sucesso");
        }

        [HttpPut("{FuncionariosId}/{TipoContactoId}")]
        public async Task<IActionResult> AtualizaContactoFuncionario(int FuncionariosId, int TipoContactoId, [FromBody] ContactoFuncionario novoContactoFuncionario)
        {
            var contactoFuncionario = await _context.ContactosFuncionarios.FirstOrDefaultAsync(a => a.FuncionariosId == FuncionariosId && a.TipoContactoId == TipoContactoId);

            if (contactoFuncionario == null)
            {
                return NotFound($"Não foi possível encontrar o contactoFornecedor com o fornecedor ID {FuncionariosId} e tipo de contacto ID {TipoContactoId}");
            }

            contactoFuncionario.Valor = novoContactoFuncionario.Valor;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o contactoFuncionario com o funcionario ID {FuncionariosId} e tipo de contacto ID {TipoContactoId}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{FuncionariosId}/{TipoContactoId}")]
        public async Task<IActionResult> RemoveContactoFuncionario(int FuncionariosId, int TipoContactoId)
        {
            var contactoFuncionario = await _context.ContactosFuncionarios.FirstOrDefaultAsync(a => a.FuncionariosId == FuncionariosId && a.TipoContactoId == TipoContactoId);

            if (contactoFuncionario == null)
            {
                return NotFound($"Não foi possível encontrar o contactoFuncionario com o funcionario ID {FuncionariosId} e tipo de contacto ID {TipoContactoId}");
            }

            _context.ContactosFuncionarios.Remove(contactoFuncionario);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o contactoFuncionario com o funcionario ID {FuncionariosId} e tipo de contacto ID {TipoContactoId}");
        }
    }
}