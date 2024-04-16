using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Servicos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacoesFuncionariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly NotificacoesServico _notificacoesService;

        public NotificacoesFuncionariosController(AppDbContext context)
        {
            _context = context;
        }

        public NotificacoesFuncionariosController(NotificacoesServico notificacoesService)
        {
            _notificacoesService = notificacoesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacaoFuncionario>>> ObterTodasNotificacoesFuncionario()
        {
            return await _context.NotificacoesFuncionario.ToListAsync();
        }
        
        [HttpGet("{NotificacaoId}/{FuncionarioId}")]
        public async Task<ActionResult<NotificacaoFuncionario>> ObterNotificacaoFuncionario(int NotificacaoId, int FuncionarioId)
        {
            var notificacaoFuncionario = await _context.NotificacoesFuncionario.FirstOrDefaultAsync(a => a.NotificacaoId == NotificacaoId && a.FuncionarioId == FuncionarioId);

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
            bool existe = await _notificacoesService.ExisteNotificacao(notificacaoFuncionario.NotificacaoId);
            if (!existe)
            {
                return BadRequest();
            }

            _context.NotificacoesFuncionario.Add(notificacaoFuncionario);
            await _context.SaveChangesAsync();

            return Ok("Funcionário notificado com sucesso");
        }

        //[HttpPut("{NotificacaoId}/{FuncionarioId}")]
        //public async Task<IActionResult> AtualizarNotificacaoFuncionario(int NotificacaoId, int FuncionarioId, [FromBody] NotificacaoFuncionario novaNotificacaoFuncionario)
        //{
        //    var notificacaoFuncionario = await _context.NotificacoesFuncionario.FirstOrDefaultAsync(a => a.NotificacaoId == NotificacaoId && a.FuncionarioId == FuncionarioId);

        //    if (notificacaoFuncionario == null)
        //    {
        //        return NotFound($"Não foi possível encontrar a notificacaoFuncionario com a notificacao ID {NotificacaoId} e o funcionario ID {FuncionarioId}");
        //    }

        //    notificacaoFuncionario.Estado = novaNotificacaoFuncionario.Estado;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //        return Ok($"NotificacaoFuncionario atualizada com sucesso para a notificacao ID {NotificacaoId} e o funcionario ID {FuncionarioId}");
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        [HttpDelete("{NotificacaoId}/{FuncionarioId}")]
        public async Task<IActionResult> RemoverNotificacaoFuncionario(int NotificacaoId, int FuncionarioId)
        {
            var notificacaoFuncionario = await _context.NotificacoesFuncionario.FirstOrDefaultAsync(a => a.NotificacaoId == NotificacaoId && a.FuncionarioId == FuncionarioId);


            if (notificacaoFuncionario == null)
            {
                return NotFound($"Notificação {NotificacaoId} do funcionario {FuncionarioId} não encontrada");
            }

            _context.NotificacoesFuncionario.Remove(notificacaoFuncionario);
            await _context.SaveChangesAsync();

            return Ok($"Notificação {NotificacaoId} removida do funcionário {FuncionarioId} com sucesso");
        }

        [HttpGet("InserirNotificacaoTipoFuncionario/{NotificacaoId}/{TipoFuncionarioId}")]
        public async Task<ActionResult<NotificacaoFuncionario>> InserirNotificacaoTipoFuncionario(int NotificacaoId, int TipoFuncionarioId)
        {
            bool existe = await _notificacoesService.ExisteNotificacao(NotificacaoId);
            if (!existe)
            {
                return BadRequest();
            }

            var funcionarios = await _context.Funcionarios.Where(f => f.TiposFuncionarioId == TipoFuncionarioId).ToListAsync();

            foreach (var funcionario in funcionarios)
            {
                var notificacaoFuncionario = new NotificacaoFuncionario
                {
                    NotificacaoId = NotificacaoId,
                    FuncionarioId = funcionario.Id,
                    Estado = 0
                };

                _context.NotificacoesFuncionario.Add(notificacaoFuncionario);
            }
            await _context.SaveChangesAsync();

            return Ok($"Funcionários do tipo {TipoFuncionarioId} notificados com sucesso");
        }

    }
}
