using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContactosResponsaveisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactosResponsaveisController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactoResponsavel>>> ObterTodosContactoResponsavel()
        {
            var contactoResponsavel = await _context.ContactosResponsaveis.ToListAsync();
            return Ok(contactoResponsavel);
        }

        [HttpGet("{ResponsaveisId}/{TipoContactoId}")]
        public async Task<ActionResult<ContactoResponsavel>> ObterContactoResponsavel(int ResponsaveisId, int TipoContactoId)
        {
            var contactoResponsavel = await _context.ContactosResponsaveis.FirstOrDefaultAsync(a => a.ResponsaveisId == ResponsaveisId && a.TipoContactoId == TipoContactoId);

            if (contactoResponsavel == null)
            {
                return NotFound();
            }
            return Ok(contactoResponsavel);
        }

        [HttpPost]
        public async Task<ActionResult<ContactoResponsavel>> InserirContactoResponsavel([FromBody] ContactoResponsavel contactoResponsavel)
        {
            if (contactoResponsavel == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.ContactosResponsaveis.Add(contactoResponsavel);
            await _context.SaveChangesAsync();

            return Ok("contactoResponsavel adicionado com sucesso");
        }

        [HttpPut("{ResponsaveisId}/{TipoContactoId}")]
        public async Task<IActionResult> AtualizaContactoResponsavel(int ResponsaveisId, int TipoContactoId, [FromBody] ContactoResponsavel novoContactoResponsavel)
        {
            var contactoResponsavel = await _context.ContactosResponsaveis.FirstOrDefaultAsync(a => a.ResponsaveisId == ResponsaveisId && a.TipoContactoId == TipoContactoId);

            if (contactoResponsavel == null)
            {
                return NotFound($"Não foi possível encontrar o contactoResponsavel com o responsavel ID {ResponsaveisId} e tipo de contacto ID {TipoContactoId}");
            }

            contactoResponsavel.Valor = novoContactoResponsavel.Valor;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o contactoResponsavel com o responsavel ID {ResponsaveisId} e tipo de contacto ID {TipoContactoId}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{ResponsaveisId}/{TipoContactoId}")]
        public async Task<IActionResult> RemoveContactoResponsavel(int ResponsaveisId, int TipoContactoId)
        {
            var contactoResponsavel = await _context.ContactosResponsaveis.FirstOrDefaultAsync(a => a.ResponsaveisId == ResponsaveisId && a.TipoContactoId == TipoContactoId);

            if (contactoResponsavel == null)
            {
                return NotFound($"Não foi possível encontrar o contactoResponsavel com o responsavel ID {ResponsaveisId} e tipo de contacto ID {TipoContactoId}");
            }

            _context.ContactosResponsaveis.Remove(contactoResponsavel);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o contactoResponsavel com o responsavel ID {ResponsaveisId} e tipo de contacto ID {TipoContactoId}");
        }
    }
}