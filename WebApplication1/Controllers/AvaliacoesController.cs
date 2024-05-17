using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        // Obter todas as avaliações com filtros especiais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> ObterTodasAvaliacoes(
            int? idMin = null, int? idMax = null,
            int? utenteId = null,
            int? funcionarioId = null,
            int? tipoAvaliacaoId = null,
            DateTime? dataMin = null, DateTime? dataMax = null)
        {
            IQueryable<Avaliacao> query = _context.Avaliacoes;

            // adicionar filtros especiais
            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (utenteId.HasValue)
            {
                query = query.Where(d => d.UtentesId == utenteId.Value);
            }

            if (funcionarioId.HasValue)
            {
                query = query.Where(d => d.FuncionariosId == funcionarioId.Value);
            }

            if (tipoAvaliacaoId.HasValue)
            {
                query = query.Where(d => d.TipoAvaliacaoId == tipoAvaliacaoId.Value);
            }


            var avaliacoesDetalhes = await (
                from avaliacao in query
                join utente in _context.Utentes on avaliacao.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on avaliacao.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join tipoAvaliacao in _context.Funcionarios on avaliacao.TipoAvaliacaoId equals tipoAvaliacao.FuncionarioID into tG
                from tipoAvaliacao in tG.DefaultIfEmpty()
                select new
                {
                    Id = avaliacao.Id,
                    UtentesId = avaliacao.UtentesId,
                    Utentes = utente.Nome,
                    FuncionarioId = avaliacao.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    Analise = avaliacao.Analise,
                    Data = avaliacao.Data,
                    TipoAvaliacaoId = avaliacao.TipoAvaliacaoId,
                    TipoAvaliacao = tipoAvaliacao.Nome,
                    AuPulmonar = avaliacao.AuscultacaoPulmonar,
                    AuCardiaca = avaliacao.AuscultacaoCardiaca
                }
            ).ToListAsync();

            return Ok(avaliacoesDetalhes);

        }

        // Metodo para obter uma avaliação a partir do id
        [HttpGet("{id}")]
        public async Task<ActionResult<Avaliacao>> ObterAvaliacao(int id)
        {
            IQueryable<Avaliacao> query = _context.Avaliacoes;
            query = query.Where(d => d.Id == id);


            var avaliacaoDetalhes = await (
                from avaliacao in query
                join utente in _context.Utentes on avaliacao.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on avaliacao.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join tipoAvaliacao in _context.TiposAvaliacao on avaliacao.TipoAvaliacaoId equals tipoAvaliacao.Id into tG
                from tipoAvaliacao in tG.DefaultIfEmpty()
                select new
                {
                    Id = avaliacao.Id,
                    UtentesId = avaliacao.UtentesId,
                    Utentes = utente.Nome,
                    FuncionarioId = avaliacao.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    Analise = avaliacao.Analise,
                    Data = avaliacao.Data,
                    TipoAvaliacaoId = avaliacao.TipoAvaliacaoId,
                    TipoAvaliacao = tipoAvaliacao.Descricao,
                    AuPulmonar = avaliacao.AuscultacaoPulmonar,
                    AuCardiaca = avaliacao.AuscultacaoCardiaca
                }
            ).ToListAsync();

            return Ok(avaliacaoDetalhes);
        }


        //Metodo para  inserir uma nova avaliação
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


        //Metodo para atualizar os dados da Avaliação
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAvaliacao(int id, [FromBody] Avaliacao novaAvaliacao)
        {
            var avaliacao = await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.Id == id);

            if (avaliacao == null)
            {
                return NotFound($"Não foi possível encontrar a avaliação com o ID {id}");
            }

            if (avaliacao.UtentesId != novaAvaliacao.UtentesId)
            {
                return NotFound($"Erro, tentativa de alteração de avaliação de outro utente.");
            }

            avaliacao.Analise = novaAvaliacao.Analise;
            avaliacao.TipoAvaliacaoId = novaAvaliacao.TipoAvaliacaoId;
            avaliacao.AuscultacaoPulmonar = novaAvaliacao.AuscultacaoPulmonar;
            avaliacao.AuscultacaoCardiaca = novaAvaliacao.AuscultacaoCardiaca;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Avaliação atualizada para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //Metodo para remover uma Avaliação
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.Id == id);

            if (avaliacao == null)
            {
                return NotFound($"Não foi possível encontrar a avaliação para com o ID {id}");
            }

            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();

            return Ok($"Avaliação com o ID {id} removida");
        }
    }
}
