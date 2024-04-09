using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacoesResponsavelController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificacoesResponsavelController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacaoResponsavel>>> ObterTodasNotificacoesResponsavel()
        {
            return await _context.Notificacoes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificacaoResponsavel>> ObterNotificacaoResponsavel(int id)
        {
            var notificacaoResponsavel = await _context.NotificacoesResponsavel.FindAsync(id);

            if (notificacaoResponsavel == null)
            {
                return NotFound();
            }

            return notificacaoResponsavel;
        }

        [HttpPost]
        public async Task<ActionResult<NotificacaoResponsavel>> InserirNotificacaoResponsavel([FromBody] NotificacaoResponsavel notificacaoResponsavel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Objeto inválido");
            }

            _context.NotificacoesResponsavel.Add(notificacaoResponsavel);
            await _context.SaveChangesAsync();

            return Ok("NotificacaoResponsavel adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarNotificacaoResponsavel(int id, [FromBody] NotificacaoResponsavel novaNotificacaoResponsavel)
        {
            var notificacaoResponsavel = await _context.NotificacoesResponsavel.FindAsync(id);

            if (notificacaoResponsavel == null)
            {
                return NotFound($"Não foi possível encontrar a notificacaoResponsavel com o ID {id}");
            }

            notificacaoResponsavel.Mensagem = novaNotificacaoResponsavel.Mensagem;
            notificacaoResponsavel.Estado = novaNotificacaoResponsavel.Estado;
            notificacaoResponsavel.ResponsaveisId = novaNotificacaoResponsavel.ResponsaveisId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"NotificacaoResponsavel atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverNotificacaoResponsavel(int id)
        {
            var notificacaoResponsavel = await _context.NotificacoesResponsavel.FindAsync(id);

            if (notificacaoResponsavel == null)
            {
                return NotFound($"NotificacaoResponsavel com o ID {id} não encontrada");
            }

            _context.NotificacoesResponsavel.Remove(notificacaoResponsavel);
            await _context.SaveChangesAsync();

            return Ok($"NotificacaoResponsavel com o ID {id} removida com sucesso");
        }

    }
}