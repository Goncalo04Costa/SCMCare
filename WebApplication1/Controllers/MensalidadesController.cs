using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensalidadesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MensalidadesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mensalidade>>> ObterTodasMensalidades(
            int? utentesIdMin = null, int? utentesIdMax = null,
            int? tiposPagamentoId = null,
            int? estado = null,
            DateTime? mesMin = null, DateTime? mesMax = null)
        {
            IQueryable<Mensalidade> query = _context.Mensalidades;

            if (utentesIdMin.HasValue)
            {
                query = query.Where(d => d.UtentesId >= utentesIdMin.Value);
            }

            if (utentesIdMax.HasValue)
            {
                query = query.Where(d => d.UtentesId <= utentesIdMax.Value);
            }

            if (mesMin.HasValue)
            {
                query = query.Where(d => d.Mes >= mesMin.Value);
            }

            if (mesMax.HasValue)
            {
                query = query.Where(d => d.Mes <= mesMax.Value);
            }

            if (tiposPagamentoId.HasValue)
            {
                query = query.Where(d => d.TiposPagamentoId == tiposPagamentoId.Value);
            }

            if (estado.HasValue)
            {
                query = query.Where(d => d.Estado == estado.Value);
            }


            var mensalidadesDetalhes = await (
                from mensalidade in query
                join utente in _context.Utentes on mensalidade.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join tipoPagamento in _context.TiposPagamento on mensalidade.TiposPagamentoId equals tipoPagamento.Id into tG
                from tipoPagamento in tG.DefaultIfEmpty()
                select new
                {
                    Mes = mensalidade.Mes,
                    DataPagamento = mensalidade.DataPagamento,
                    UtentesId = mensalidade.UtentesId,
                    Utentes = utente.Nome,
                    TiposPagamentoId = mensalidade.TiposPagamentoId,
                    TiposPagamento = tipoPagamento.Descricao,
                    Estado = mensalidade.Estado
                }
            ).ToListAsync();

            return Ok(mensalidadesDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mensalidade>> ObterMensalidade(DateTime mes, int utentesId)
        {
            IQueryable<Mensalidade> query = _context.Mensalidades;
            query = query.Where(d => d.UtentesId == utentesId && d.Mes == mes);


            var mensalidadeDetalhes = await (
                from mensalidade in query
                join utente in _context.Utentes on mensalidade.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join tipoPagamento in _context.TiposPagamento on mensalidade.TiposPagamentoId equals tipoPagamento.Id into tG
                from tipoPagamento in tG.DefaultIfEmpty()
                select new
                {
                    Mes = mensalidade.Mes,
                    DataPagamento = mensalidade.DataPagamento,
                    UtentesId = mensalidade.UtentesId,
                    Utentes = utente.Nome,
                    TiposPagamentoId = mensalidade.TiposPagamentoId,
                    TiposPagamento = tipoPagamento.Descricao,
                    Estado = mensalidade.Estado
                }
            ).ToListAsync();

            return Ok(mensalidadeDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Mensalidade>> InserirMensalidade([FromBody] Mensalidade mensalidade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Mensalidades.Add(mensalidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterMensalidade), new { mes = mensalidade.Mes, utentesId = mensalidade.UtentesId }, mensalidade);
        }

        [HttpPut("{mes}/{utentesId}")]
        public async Task<IActionResult> AtualizarMensalidade(DateTime mes, int utentesId, [FromBody] Mensalidade mensalidade)
        {
            if (mes != mensalidade.Mes || utentesId != mensalidade.UtentesId)
            {
                return BadRequest();
            }

            _context.Entry(mensalidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensalidadeExists(mes, utentesId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{mes}/{utentesId}")]
        public async Task<IActionResult> RemoverMensalidade(DateTime mes, int utentesId)
        {
            var mensalidade = await _context.Mensalidades.FirstOrDefaultAsync(m => m.Mes == mes && m.UtentesId == utentesId);
            if (mensalidade == null)
            {
                return NotFound();
            }

            _context.Mensalidades.Remove(mensalidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MensalidadeExists(DateTime mes, int utentesId)
        {
            return _context.Mensalidades.Any(m => m.Mes == mes && m.UtentesId == utentesId);
        }

        [HttpGet("estado/{mes}/{utentesId}")]
        public async Task<ActionResult<string>> ObterEstadoMensalidade(DateTime mes, int utentesId)
        {
            try
            {
                // Busca a mensalidade com base no mês e no ID do utente
                var mensalidade = await _context.Mensalidades.FirstOrDefaultAsync(m => m.Mes == mes && m.UtentesId == utentesId);

                if (mensalidade == null)
                {
                    return NotFound($"Não foi encontrada nenhuma mensalidade para o mês {mes} e o utente com ID {utentesId}.");
                }

                // Define o estado da mensalidade com base no valor do campo "Estado"
                string estado = mensalidade.Estado == 0 ? "Pendente" : mensalidade.Estado == 1 ? "Pago" : "Estado inválido";

                return Ok($"O estado da mensalidade para o mês {mes} e o utente com ID {utentesId} é: {estado}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao obter o estado da mensalidade: {ex.Message}");
            }
        }

    }
}
