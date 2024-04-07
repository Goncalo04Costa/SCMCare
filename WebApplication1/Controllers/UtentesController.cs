using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UtentesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UtentesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utente>>> ObterTodosUtentes(
            int? idMin = null, int? idMax = null,
            int? nifMin = null, int? nifMax = null,
            int? snsMin = null, int? snsMax = null,
            string nomeMin = null, string nomeMax = null,
            DateTime? dataMin = null, DateTime? dataMax = null, //Data de admissão
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
                query = query.Where(d => !d.Historico); // Mostra utentes que não estão em histórico
            }

            else if (!historico0 && historico1)
            {
                query = query.Where(d => d.Historico); // Mostra utentes que estão em histórico
            }

            if (tipo0 && !tipo1)
            {
                query = query.Where(d => !d.Tipo); // Mostra utentes com tipo 0
            }

            else if (!tipo0 && tipo1)
            {
                query = query.Where(d => d.Tipo); // Mostra utentes com tipo 1
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

        [HttpPost]
        public async Task<ActionResult<Utente>> InserirUtente([FromBody] Utente utente)
        {
            if (utente == null)
            {
                return BadRequest("Objeto inválido");
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
    }
}
