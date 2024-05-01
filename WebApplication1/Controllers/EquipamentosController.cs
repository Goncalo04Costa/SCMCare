using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipamentosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipamento>>> ObterTodosEquipamentos(
            int? idMin = null, int? idMax = null,
            string? descMin = null, string? descMax = null,
            bool historico0 = false, bool historico1 = false,
            int? tipoEquipamentoId = null,
            int? quartoId = null)
        {
            IQueryable<Equipamento> query = _context.Equipamentos;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(descMin))
            {
                query = query.Where(d => d.Descricao.CompareTo(descMin) >= 0);
            }

            if (!string.IsNullOrEmpty(descMax))
            {
                query = query.Where(d => d.Descricao.CompareTo(descMax + "ZZZ") <= 0);
            }

            if (historico0 && !historico1)
            {
                query = query.Where(d => !d.Historico);
            }
            else if (!historico0 && historico1)
            {
                query = query.Where(d => d.Historico);
            }

            if (tipoEquipamentoId.HasValue)
            {
                query = query.Where(d => d.TiposEquipamentoId == tipoEquipamentoId.Value);
            }

            if (quartoId.HasValue)
            {
                query = query.Where(d => d.QuartosId == quartoId.Value);
            }


            var equipamentosDetalhes = await (
                from equipamento in query
                join tipoEquipamento in _context.TiposEquipamento on equipamento.TiposEquipamentoId equals tipoEquipamento.Id into tG
                from tipoEquipamento in tG.DefaultIfEmpty()
                join quarto in _context.Quartos on equipamento.QuartosId equals quarto.Id into qG
                from quarto in qG.DefaultIfEmpty()
                select new
                {
                    Id = equipamento.Id,
                    Descricao = equipamento.Descricao,
                    Historico = equipamento.Historico,
                    TipoEquipamentoId = equipamento.TiposEquipamentoId,
                    TipoEquipamento = tipoEquipamento.Descricao,
                    QuartoId = equipamento.QuartosId,
                    Quarto = quarto.Id
                }
            ).ToListAsync();

            return Ok(equipamentosDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Equipamento>> ObterEquipamento(int id)
        {
            IQueryable<Equipamento> query = _context.Equipamentos;
            query = query.Where(d => d.Id == id);


            var equipamentoDetalhes = await (
                from equipamento in query
                join tipoEquipamento in _context.TiposEquipamento on equipamento.TiposEquipamentoId equals tipoEquipamento.Id into tG
                from tipoEquipamento in tG.DefaultIfEmpty()
                join quarto in _context.Quartos on equipamento.QuartosId equals quarto.Id into qG
                from quarto in qG.DefaultIfEmpty()
                select new
                {
                    Id = equipamento.Id,
                    Descricao = equipamento.Descricao,
                    Historico = equipamento.Historico,
                    TipoEquipamentoId = equipamento.TiposEquipamentoId,
                    TipoEquipamento = tipoEquipamento.Descricao,
                    QuartoId = equipamento.QuartosId,
                    Quarto = quarto.Id
                }
            ).ToListAsync();

            return Ok(equipamentoDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Equipamento>> InserirEquipamento([FromBody] Equipamento equipamento)
        {
            if (equipamento == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Equipamentos.Add(equipamento);
            await _context.SaveChangesAsync();

            return Ok("Equipamento adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEquipamento(int id, [FromBody] Equipamento novoEquipamento)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
            {
                return NotFound($"Não foi possível encontrar o equipamento com o ID {id}");
            }

            equipamento.Descricao = novoEquipamento.Descricao;
            equipamento.Historico = novoEquipamento.Historico;
            equipamento.TiposEquipamentoId = novoEquipamento.TiposEquipamentoId;
            equipamento.QuartosId = novoEquipamento.QuartosId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Equipamento atualizado com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverEquipamento(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
            {
                return NotFound($"Equipamento com o ID {id} não encontrado");
            }

            _context.Equipamentos.Remove(equipamento);
            await _context.SaveChangesAsync();

            return Ok($"Equipamento com o ID {id} removido com sucesso");
        }
    }
}
