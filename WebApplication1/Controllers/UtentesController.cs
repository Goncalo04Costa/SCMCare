using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using RegrasNegocio; // Importe o namespace onde a classe RegrasUtentes está definida
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtentesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RegrasUtentes _regrasUtentes; 

        public UtentesController(AppDbContext context, RegrasUtentes regrasUtentes)
        {
            _context = context;
            _regrasUtentes = regrasUtentes;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utente>>> ObterTodosUtentes(
            int? idMin = null, int? idMax = null,
            int? nifMin = null, int? nifMax = null,
            int? snsMin = null, int? snsMax = null,
            string nomeMin = null, string nomeMax = null,
            DateTime? dataMin = null, DateTime? dataMax = null, 
            bool historico0 = false, bool historico1 = false,
            bool tipo0 = false, bool tipo1 = false)
        {
            IQueryable<Utente> query = _context.Utentes;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (nifMin.HasValue)
            {
                query = query.Where(d => d.NIF >= nifMin.Value);
            }

            if (nifMax.HasValue)
            {
                query = query.Where(d => d.NIF <= nifMax.Value);
            }

            if (snsMin.HasValue)
            {
                query = query.Where(d => d.SNS >= snsMin.Value);
            }

            if (snsMax.HasValue)
            {
                query = query.Where(d => d.SNS <= snsMax.Value);
            }

            if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMin) >= 0);
            }

            if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMax + "ZZZ") <= 0);
            }

            if (dataMin.HasValue)
            {
                query = query.Where(d => d.DataAdmissao >= dataMin.Value);
            }

            if (dataMax.HasValue)
            {
                query = query.Where(d => d.DataAdmissao <= dataMax.Value);
            }

            if (historico0 && !historico1)
            {
                query = query.Where(d => !d.Historico);
            }

            else if (!historico0 && historico1)
            {
                query = query.Where(d => d.Historico);
            }

            if (tipo0 && !tipo1)
            {
                query = query.Where(d => !d.Tipo);
            }

            else if (!tipo0 && tipo1)
            {
                query = query.Where(d => d.Tipo);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Utente>> ObterUtente(int id)
        {
            var dado = await _context.Utentes.FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound();
            }
            return Ok(dado);
        }

        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<Utente>> ObterUtentePorNome(string nome)
        {
            try
            {
                var utente = await _context.Utentes.FirstOrDefaultAsync(d => d.Nome == nome);

                if (utente == null)
                {
                    return NotFound($"Utente com o nome '{nome}' não encontrado.");
                }

                return Ok(utente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao obter o utente pelo nome: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Utente>> InserirUtente([FromBody] Utente utente)
        {
            if (utente == null)
            {
                return BadRequest("Objeto inválido");
            }

            
            bool nifExiste = await _regrasUtentes.VerificarNIFExistente(utente.NIF);
            if (nifExiste)
            {
                return BadRequest("NIF já existe.");
            }

            _context.Utentes.Add(utente);
            await _context.SaveChangesAsync();

            return Ok("Utente adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaUtente(int id, [FromBody] Utente novoUtente)
        {
            var utente = await _context.Utentes.FirstOrDefaultAsync(d => d.Id == id);
            if (utente == null)
            {
                return NotFound($"Não foi possível encontrar o utente com o ID {id}");
            }

            utente.Nome = novoUtente.Nome;
            utente.NIF = novoUtente.NIF;
            utente.SNS = novoUtente.SNS;
            utente.DataAdmissao = novoUtente.DataAdmissao;
            utente.DataNascimento = novoUtente.DataNascimento;
            utente.Historico = novoUtente.Historico;
            utente.Tipo = novoUtente.Tipo;
            utente.TiposAdmissaoId = novoUtente.TiposAdmissaoId;
            utente.MotivoAdmissao = novoUtente.MotivoAdmissao;
            utente.DiagnosticoAdmissao = novoUtente.DiagnosticoAdmissao;
            utente.Observacoes = novoUtente.Observacoes;
            utente.NotaAdmissao = novoUtente.NotaAdmissao;
            utente.AntecedentesPessoais = novoUtente.AntecedentesPessoais;
            utente.ExameObjetivo = novoUtente.ExameObjetivo;
            utente.Mensalidade = novoUtente.Mensalidade;
            utente.Cofinanciamento = novoUtente.Cofinanciamento;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o utente com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUtente(int id)
        {
            var utente = await _context.Utentes.FirstOrDefaultAsync(d => d.Id == id);
            if (utente == null)
            {
                return NotFound($"Não foi possível encontrar o utente com o ID {id}");
            }

            _context.Utentes.Remove(utente);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o utente com o ID {id}");
        }

        [HttpGet("{id}/ficha")]
        public async Task<IActionResult> ImprimirFichaUtente(int id)
        {
            var utente = await _context.Utentes.FindAsync(id);

            if (utente == null)
            {
                return NotFound($"Não foi possível encontrar o utente com o ID {id}");
            }

            var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new iText.Layout.Document(pdf);

            document.Add(new Paragraph($"Ficha do Utente - {utente.Nome}"));
            document.Add(new Paragraph($"ID: {utente.Id}"));
            document.Add(new Paragraph($"NIF: {utente.NIF}"));
            document.Add(new Paragraph($"SNS: {utente.SNS}"));
            document.Add(new Paragraph($"Data de Admissão: {utente.DataAdmissao}"));

            document.Close();

            return File(memoryStream.ToArray(), "application/pdf", $"ficha_utente_{utente.Id}.pdf");
        }
    }
}
