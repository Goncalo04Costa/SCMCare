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

        public NotificacoesFuncionariosController(AppDbContext context, NotificacoesServico notificacoesService)
        {
            _context = context;
            _notificacoesService = notificacoesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacaoFuncionario>>> ObterTodasNotificacoesFuncionario(
            int? funcionarioId = null, int? notificacaoId = null,
            int? estado = null)
        {
            IQueryable<NotificacaoFuncionario> query = _context.NotificacoesFuncionarios;

            if (funcionarioId.HasValue)
            {
                query = query.Where(d => d.FuncionariosId >= funcionarioId.Value);
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

        [HttpGet("{NotificacoesId}/{FuncionariosId}")]
        public async Task<ActionResult<NotificacaoFuncionario>> ObterNotificacaoFuncionario(int NotificacoesId, int FuncionariosId)
        {
            var notificacaoFuncionario = await _context.NotificacoesFuncionarios.FirstOrDefaultAsync(a => a.NotificacoesId == NotificacoesId && a.FuncionariosId == FuncionariosId);

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
                return BadRequest("Objeto inv�lido");
            }
            bool existe = await _notificacoesService.ExisteNotificacao(notificacaoFuncionario.NotificacoesId);
            if (!existe)
            {
                return BadRequest();
            }

            _context.NotificacoesFuncionarios.Add(notificacaoFuncionario);
            await _context.SaveChangesAsync();

            return Ok("Funcion�rio notificado com sucesso");
        }

        //[HttpPut("{NotificacoesId}/{FuncionariosId}")]
        //public async Task<IActionResult> AtualizarNotificacaoFuncionario(int NotificacoesId, int FuncionariosId, [FromBody] NotificacaoFuncionario novaNotificacaoFuncionario)
        //{
        //    var notificacaoFuncionario = await _context.NotificacoesFuncionario.FirstOrDefaultAsync(a => a.NotificacoesId == NotificacoesId && a.FuncionariosId == FuncionariosId);

        //    if (notificacaoFuncionario == null)
        //    {
        //        return NotFound($"N�o foi poss�vel encontrar a notificacaoFuncionario com a notificacao ID {NotificacoesId} e o funcionario ID {FuncionariosId}");
        //    }

        //    notificacaoFuncionario.Estado = novaNotificacaoFuncionario.Estado;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //        return Ok($"NotificacaoFuncionario atualizada com sucesso para a notificacao ID {NotificacoesId} e o funcionario ID {FuncionariosId}");
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        [HttpDelete("{NotificacoesId}/{FuncionariosId}")]
        public async Task<IActionResult> RemoverNotificacaoFuncionario(int NotificacoesId, int FuncionariosId)
        {
            var notificacaoFuncionario = await _context.NotificacoesFuncionarios.FirstOrDefaultAsync(a => a.NotificacoesId == NotificacoesId && a.FuncionariosId == FuncionariosId);


            if (notificacaoFuncionario == null)
            {
                return NotFound($"Notifica��o {NotificacoesId} do funcionario {FuncionariosId} n�o encontrada");
            }

            _context.NotificacoesFuncionarios.Remove(notificacaoFuncionario);
            await _context.SaveChangesAsync();

            return Ok($"Notifica��o {NotificacoesId} removida do funcion�rio {FuncionariosId} com sucesso");
        }

        [HttpGet("InserirNotificacaoTipoFuncionario/{NotificacoesId}/{TipoFuncionariosId}")]
        public async Task<ActionResult<NotificacaoFuncionario>> InserirNotificacaoTipoFuncionario(int NotificacoesId, int TipoFuncionariosId)
        {
            bool existe = await _notificacoesService.ExisteNotificacao(NotificacoesId);
            if (!existe)
            {
                return BadRequest();
            }

            var funcionarios = await _context.Funcionarios.Where(f => f.TiposFuncionarioId == TipoFuncionariosId).ToListAsync();

            foreach (var funcionario in funcionarios)
            {
                var notificacaoFuncionario = new NotificacaoFuncionario
                {
                    NotificacoesId = NotificacoesId,
                    FuncionariosId = funcionario.FuncionarioID,
                    Estado = 0
                };

                _context.NotificacoesFuncionarios.Add(notificacaoFuncionario);
            }
            await _context.SaveChangesAsync();

            return Ok($"Funcion�rios do tipo {TipoFuncionariosId} notificados com sucesso");
        }

    }
}
