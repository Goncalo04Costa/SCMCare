using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Servicos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacoesResponsavelController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly NotificacoesServico _notificacoesService;

        public NotificacoesResponsavelController(AppDbContext context, NotificacoesServico _notificacoesService)
        {
            _context = context;
        }

        public NotificacoesResponsavelController(NotificacoesServico notificacoesService)
        {
            _notificacoesService = notificacoesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacaoResponsavel>>> ObterTodasNotificacoesResponsavel()
        {
            return await _context.NotificacoesResponsavel.ToListAsync();
        }

        [HttpGet("{NotificacaoId}/{ResponsavelId}")]
        public async Task<ActionResult<NotificacaoResponsavel>> ObterNotificacaoResponsavel(int NotificacaoId, int ResponsavelId)
        {
            var notificacaoResponsavel = await _context.NotificacoesResponsavel.FirstOrDefaultAsync(a => a.NotificacaoId == NotificacaoId && a.ResponsavelId == ResponsavelId);

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

            bool existe = await _notificacoesService.ExisteNotificacao(notificacaoResponsavel.NotificacaoId);
            if (!existe)
            { 
                return BadRequest();
            }

            _context.NotificacoesResponsavel.Add(notificacaoResponsavel);
            await _context.SaveChangesAsync();

            return Ok("Responsável notificado com sucesso");
        }

        [HttpDelete("{NotificacaoId}/{ResponsavelId}")]
        public async Task<IActionResult> RemoverNotificacaoResponsavel(int NotificacaoId, int ResponsavelId)
        {
            var notificacaoResponsavel = await _context.NotificacoesResponsavel.FirstOrDefaultAsync(a => a.NotificacaoId == NotificacaoId && a.ResponsavelId == ResponsavelId);

            if (notificacaoResponsavel == null)
            {
                return NotFound($"Notificação {NotificacaoId} do responsável {ResponsavelId} não encontrada");
            }

            _context.NotificacoesResponsavel.Remove(notificacaoResponsavel);
            await _context.SaveChangesAsync();

            return Ok($"Notificação {NotificacaoId} removida do responsável {ResponsavelId} com sucesso");
        }

    }
}