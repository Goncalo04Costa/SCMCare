using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacoesFuncionariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificacoesFuncionariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacaoFuncionario>>> ObterTodasNotificacoesFuncionario()
        {
            return await _context.NotificacoesFuncionario.ToListAsync();
        }
        
        [HttpGet("{NotificacoesId}/{FuncionariosId}")]
        public async Task<ActionResult<NotificacaoFuncionario>> ObterNotificacaoFuncionario(int NotificacoesId, int FuncionariosId)
        {
            var notificacaoFuncionario = await _context.NotificacoesFuncionario.FirstOrDefaultAsync(a => a.NotificacoesId == NotificacoesId && a.FuncionariosId == FuncionariosId);

            if (notificacaoFuncionario == null)
            {
                return NotFound();
            }

            return notificacaoFuncionario;
        }

        [HttpPost]
        public async Task<ActionResult<NotificacaoFuncionario>> InserirNotificacaoFuncionario([FromBody] NotificacaoFuncionario notificacaoFuncionario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Objeto inválido");
            }

            _context.NotificacoesFuncionario.Add(notificacaoFuncionario);
            await _context.SaveChangesAsync();

            return Ok("NotificacaoFuncionario adicionada com sucesso");
        }

        [HttpPut("{NotificacoesId}/{FuncionariosId}")]
        public async Task<IActionResult> AtualizarNotificacaoFuncionarioo(int NotificacoesId, int FuncionariosId, [FromBody] NotificacaoFuncionario novaNotificacaoFuncionario)
        {
            var notificacaoFuncionario = await _context.NotificacoesFuncionario.FirstOrDefaultAsync(a => a.NotificacoesId == NotificacoesId && a.FuncionariosId == FuncionariosId);

            if (notificacaoFuncionario == null)
            {
                return NotFound($"Não foi possível encontrar a notificacaoFuncionario com a notificacao ID {NotificacoesId} e o funcionario ID {FuncionariosId}");
            }

            notificacaoFuncionario.Estado = novaNotificacaoFuncionario.Estado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"NotificacaoFuncionario atualizada com sucesso para a notificacao ID {NotificacoesId} e o funcionario ID {FuncionariosId}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{NotificacoesId}/{FuncionariosId}")]
        public async Task<IActionResult> RemoverNotificacaoFuncionario(int NotificacoesId, int FuncionariosId)
        {
            var notificacaoFuncionario = await _context.NotificacoesFuncionario.FirstOrDefaultAsync(a => a.NotificacoesId == NotificacoesId && a.FuncionariosId == FuncionariosId);


            if (notificacaoFuncionario == null)
            {
                return NotFound($"NotificacaoFuncionario com a notificacao ID {NotificacoesId} e o funcionario ID {FuncionariosId} não encontrada");
            }

            _context.NotificacoesFuncionario.Remove(notificacaoFuncionario);
            await _context.SaveChangesAsync();

            return Ok($"NotificacaoFuncionario com a notificacao ID {NotificacoesId} e o funcionario ID {FuncionariosId} removida com sucesso");
        }

    }
}
