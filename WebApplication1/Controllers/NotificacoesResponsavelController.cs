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

        public NotificacoesResponsavelController(AppDbContext context, NotificacoesServico notificacoesService)
        {
            _context = context;
            _notificacoesService = notificacoesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacaoResponsavel>>> ObterTodasNotificacoesResponsavel(
            int? responsavelId = null, int? notificacaoId = null,
            int? estado = null)
        {
            IQueryable<NotificacaoResponsavel> query = _context.NotificacoesResponsaveis;

            if (responsavelId.HasValue)
            {
                query = query.Where(d => d.ResponsaveisId >= responsavelId.Value);
            }

            if (notificacaoId.HasValue)
            {
                query = query.Where(d => d.NotificacoesId >= notificacaoId.Value);
            }

            if (estado.HasValue)
            {
                query = query.Where(d => d.Estado >= estado.Value);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{NotificacoesId}/{ResponsaveisId}")]
        public async Task<ActionResult<NotificacaoResponsavel>> ObterNotificacaoResponsavel(int NotificacoesId, int ResponsaveisId)
        {
            var notificacaoResponsavel = await _context.NotificacoesResponsaveis.FirstOrDefaultAsync(a => a.NotificacoesId == NotificacoesId && a.ResponsaveisId == ResponsaveisId);

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

            bool existe = await _notificacoesService.ExisteNotificacao(notificacaoResponsavel.NotificacoesId);
            if (!existe)
            { 
                return BadRequest();
            }

            _context.NotificacoesResponsaveis.Add(notificacaoResponsavel);
            await _context.SaveChangesAsync();

            return Ok("Responsável notificado com sucesso");
        }

        [HttpDelete("{NotificacoesId}/{ResponsaveisId}")]
        public async Task<IActionResult> RemoverNotificacaoResponsavel(int NotificacoesId, int ResponsaveisId)
        {
            var notificacaoResponsavel = await _context.NotificacoesResponsaveis.FirstOrDefaultAsync(a => a.NotificacoesId == NotificacoesId && a.ResponsaveisId == ResponsaveisId);

            if (notificacaoResponsavel == null)
            {
                return NotFound($"Notificação {NotificacoesId} do responsável {ResponsaveisId} não encontrada");
            }

            _context.NotificacoesResponsaveis.Remove(notificacaoResponsavel);
            await _context.SaveChangesAsync();

            return Ok($"Notificação {NotificacoesId} removida do responsável {ResponsaveisId} com sucesso");
        }

    }
}