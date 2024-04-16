using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Servicos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvariasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TiposFuncionarioServico _tiposFuncionarioService;
        private readonly NotificacoesServico _notificacoesService;

        public AvariasController(AppDbContext context)
        {
            _context = context;
        }

        public AvariasController(TiposFuncionarioServico tiposFuncionarioService)
        {
            _tiposFuncionarioService = tiposFuncionarioService;
        }

        public AvariasController(NotificacoesServico notificacoesService)
        {
            _notificacoesService = notificacoesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avaria>>> ObterTodasAvarias()
        {
            var avarias = await _context.Avarias.ToListAsync();
            return Ok(avarias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Avaria>> ObterAvaria(int id)
        {
            var avaria = await _context.Avarias.FindAsync(id);

            if (avaria == null)
            {
                return NotFound();
            }

            return Ok(avaria);
        }

        [HttpPost]
        public async Task<ActionResult<Avaria>> InserirAvaria([FromBody] Avaria avaria)
        {
            if (avaria == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Avarias.Add(avaria);
            await _context.SaveChangesAsync();

            int i = await _tiposFuncionarioService.ObterTipoPorNome("Engenheiro");

            if (i == -1)
                return Ok("Avaria adicionada com sucesso, com erro de notificação");

            Notificacao notificacao = new Notificacao();
            notificacao.Mensagem = $"Aberta nova avaria com o id {avaria.Id} no equipamento {avaria.EquipamentosId}: {avaria.Descricao}";
            notificacao.Data = DateTime.Now;

            int n = await _notificacoesService.InserirNotificacao(notificacao);

            if (n == 1)
                return Ok("Avaria adicionada com sucesso e engenheiros notificados");
            else
                return Ok("Avaria adicionada com sucesso, com erro de notificação");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAvaria(int id, [FromBody] Avaria novaAvaria)
        {
            var avaria = await _context.Avarias.FindAsync(id);

            if (avaria == null)
            {
                return NotFound($"Não foi possível encontrar a avaria com o ID {id}");
            }

            avaria.Data = novaAvaria.Data;
            avaria.EquipamentosId = novaAvaria.EquipamentosId;
            avaria.Descricao = novaAvaria.Descricao;
            avaria.Estado = novaAvaria.Estado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Avaria atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverAvaria(int id)
        {
            var avaria = await _context.Avarias.FindAsync(id);

            if (avaria == null)
            {
                return NotFound($"Avaria com o ID {id} não encontrada");
            }

            _context.Avarias.Remove(avaria);
            await _context.SaveChangesAsync();

            return Ok($"Avaria com o ID {id} removida com sucesso");
        }
        
        [HttpPut("AtualizarDataAvaria/{id}")]
        public async Task<IActionResult> AtualizarDataAvaria(int id, DateTime data)
        {
            var avaria = await _context.Avarias.FindAsync(id);

            if (avaria == null)
            {
                return NotFound($"Não foi possível encontrar a avaria com o ID {id}");
            }

            avaria.Data = data;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Avaria atualizada com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
