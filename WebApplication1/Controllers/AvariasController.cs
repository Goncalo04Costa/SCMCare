using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Servicos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        // Metodo para obter todas as avarias com filtros especificos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avaria>>> ObterTodasAvarias(
            int? idMin = null, int? idMax = null,
            DateTime? dataMin = null, DateTime? dataMax = null,
            int? equipamentoId = null,
            int? estado = null)
        {
            IQueryable<Avaria> query = _context.Avarias;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (dataMin.HasValue)
            {
                query = query.Where(d => d.Data >= dataMin.Value);
            }

            if (dataMax.HasValue)
            {
                query = query.Where(d => d.Data <= dataMax.Value);
            }

            if (equipamentoId.HasValue)
            {
                query = query.Where(d => d.EquipamentosId == equipamentoId.Value);
            }

            if (estado.HasValue)
            {
                query = query.Where(d => d.Estado == estado.Value);
            }


            var avariasDetalhes = await (
                from avaria in query
                join equipamento in _context.TiposEquipamento on avaria.EquipamentosId equals equipamento.Id into eG
                from equipamento in eG.DefaultIfEmpty()
                select new
                {
                    Id = avaria.Id,
                    Data = avaria.Data,
                    EquipamentoId = avaria.EquipamentosId,
                    Equipamento = equipamento.Descricao,
                    Descricao = avaria.Descricao,
                    Estado = avaria.Estado
                }
            ).ToListAsync();

            return Ok(avariasDetalhes);
        }


        // Metodo para obter uma avaria existente pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Avaria>> ObterAvaria(int id)
        {
            IQueryable<Avaria> query = _context.Avarias;
            query = query.Where(d => d.Id == id);


            var avariaDetalhes = await (
                from avaria in query
                join equipamento in _context.TiposEquipamento on avaria.EquipamentosId equals equipamento.Id into eG
                from equipamento in eG.DefaultIfEmpty()
                select new
                {
                    Id = avaria.Id,
                    Data = avaria.Data,
                    EquipamentoId = avaria.EquipamentosId,
                    Equipamento = equipamento.Descricao,
                    Descricao = avaria.Descricao,
                    Estado = avaria.Estado
                }
            ).ToListAsync();

            return Ok(avariaDetalhes);
        }


        //Metodo para inserir uma nova avaria
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


        //Metodo para atualizar dados de uma avaria
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


        //Metodo para remover uma avaria
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
        
        //Metodo para mudar a data de agendamentto da avaria
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
