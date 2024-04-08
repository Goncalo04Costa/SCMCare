﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;


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
        public async Task<ActionResult<IEnumerable<Mensalidade>>> ObterTodasMensalidades()
        {
            return await _context.Mensalidades.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mensalidade>> ObterMensalidade(DateTime mes, int utentesId)
        {
            var mensalidade = await _context.Mensalidades.FirstOrDefaultAsync(m => m.Mes == mes && m.UtentesId == utentesId);

            if (mensalidade == null)
            {
                return NotFound();
            }

            return mensalidade;
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