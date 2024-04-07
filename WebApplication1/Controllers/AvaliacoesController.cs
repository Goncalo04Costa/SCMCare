using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvaliacoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> ObterTodasAvaliacoes()
        {
            var avaliacoes = await _context.Avaliacoes.ToListAsync();
            return Ok(avaliacoes);
        }

        [HttpGet("{utenteId}/{funcionarioId}/{data}")]
        public async Task<ActionResult<Avaliacao>> ObterAvaliacao(int utenteId, int funcionarioId, DateTime data)
        {
            var avaliacao = await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.UtentesId == utenteId && a.FuncionariosId == funcionarioId && a.Data == data);

            if (avaliacao == null)
            {
                return NotFound();
            }

            return Ok(avaliacao);
        }

        [HttpPost]
        public async Task<ActionResult<Avaliacao>> InserirAvaliacao([FromBody] Avaliacao avaliacao)
        {
            if (avaliacao == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            return Ok("Avaliação adicionada com sucesso");
        }

        [HttpPut("{utenteId}/{funcionarioId}/{data}")]
        public async Task<IActionResult> AtualizarAvaliacao(int utenteId, int funcionarioId, DateTime data, [FromBody] Avaliacao novaAvaliacao)
        {
            var avaliacao = await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.UtentesId == utenteId && a.FuncionariosId == funcionarioId && a.Data == data);

            if (avaliacao == null)
            {
                return NotFound($"Não foi possível encontrar a avaliação para o utente ID {utenteId}, funcionário ID {funcionarioId} e data {data}");
            }

            avaliacao.Analise = novaAvaliacao.Analise;
            avaliacao.TipoAvaliacaoId = novaAvaliacao.TipoAvaliacaoId;
            avaliacao.AuscultacaoPulmonar = novaAvaliacao.AuscultacaoPulmonar;
            avaliacao.AuscultacaoCardiaca = novaAvaliacao.AuscultacaoCardiaca;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Avaliação atualizada para o utente ID {utenteId}, funcionário ID {funcionarioId} e data {data}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{utenteId}/{funcionarioId}/{data}")]
        public async Task<IActionResult> RemoverAvaliacao(int utenteId, int funcionarioId, DateTime data)
        {
            var avaliacao = await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.UtentesId == utenteId && a.FuncionariosId == funcionarioId && a.Data == data);

            if (avaliacao == null)
            {
                return NotFound($"Não foi possível encontrar a avaliação para o utente ID {utenteId}, funcionário ID {funcionarioId} e data {data}");
            }

            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();

            return Ok($"Avaliação removida para o utente ID {utenteId}, funcionário ID {funcionarioId} e data {data}");
        }
    }
}
