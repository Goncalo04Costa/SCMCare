using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificacoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notificacao>>> ObterTodasNotificacoes()
        {
            return await _context.Notificacoes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notificacao>> ObterNotificacao(int id)
        {
            var notificacao = await _context.Notificacoes.FindAsync(id);

            if (notificacao == null)
            {
                return NotFound();
            }

            return notificacao;
        }

        [HttpPost]
        public async Task<ActionResult<Notificacao>> InserirNotificacao([FromBody] Notificacao notificacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Objeto inv�lido");
            }

            _context.Notificacoes.Add(notificacao);
            await _context.SaveChangesAsync();

            return Ok("Notifica��o adicionada com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarNotificacao(int id, [FromBody] Notificacao novaNotificacao)
        {
            var notificacao = await _context.Notificacoes.FindAsync(id);

            if (notificacao == null)
            {
                return NotFound($"N�o foi poss�vel encontrar a notifica��o com o ID {id}");
            }

            notificacao.Mensagem = novaNotificacao.Mensagem;
            notificacao.TiposFuncionarioId = novaNotificacao.TiposFuncionarioId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Notifica��o atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverNotificacao(int id)
        {
            var notificacao = await _context.Notificacoes.FindAsync(id);

            if (notificacao == null)
            {
                return NotFound($"Notifica��o com o ID {id} n�o encontrada");
            }

            _context.Notificacoes.Remove(notificacao);
            await _context.SaveChangesAsync();

            return Ok($"Notifica��o com o ID {id} removida com sucesso");
        }

        [HttpGet("ExisteNotificacao/{id}")]
        public async Task<ActionResult<bool>> ExisteNotificacao(int id)
        {
            var notificacao = await _context.Notificacoes.FindAsync(id);

            if (notificacao == null)
            {
                return false;
            }

            return true;
        }

    }
}