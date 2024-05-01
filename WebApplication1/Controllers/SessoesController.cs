using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SessoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sessao>>> ObterSessoes(
            int? idMin = null, int? idMax = null,
            int? tipoSessaoId = null,
            int? utentesId = null,
            int? funcionariosId = null,
            DateTime? diaMin = null, DateTime? diaMax = null)
        {
            IQueryable<Sessao> query = _context.Sessoes;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (diaMin.HasValue)
            {
                query = query.Where(d => d.Dia >= diaMin.Value);
            }

            if (diaMax.HasValue)
            {
                query = query.Where(d => d.Dia <= diaMax.Value);
            }

            if (tipoSessaoId.HasValue)
            {
                query = query.Where(d => d.TiposSessaoId == tipoSessaoId.Value);
            }

            if (utentesId.HasValue)
            {
                query = query.Where(d => d.UtentesId == utentesId.Value);
            }

            if (funcionariosId.HasValue)
            {
                query = query.Where(d => d.FuncionariosId == funcionariosId.Value);
            }


            var sessoesDetalhes = await (
                from sessao in query
                join tiposessao in _context.TiposSessao on sessao.TiposSessaoId equals tiposessao.Id into tG
                from tiposessao in tG.DefaultIfEmpty()
                join utente in _context.Utentes on sessao.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on sessao.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                select new
                {
                    Id = sessao.Id,
                    TipoSessaoId = sessao.TiposSessaoId,
                    TipoSessao = tiposessao.Descricao,
                    FuncionarioId = sessao.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    UtenteId = sessao.UtentesId,
                    Utente = utente.Nome,
                    Dia = sessao.Dia,
                    Observacoes = sessao.Observacoes
                }
            ).ToListAsync();

            return Ok(sessoesDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sessao>> ObterSessao(int id)
        {
            IQueryable<Sessao> query = _context.Sessoes;
            query = query.Where(d => d.Id == id);


            var sessaoDetalhes = await (
                from sessao in query
                join tiposessao in _context.TiposSessao on sessao.TiposSessaoId equals tiposessao.Id into tG
                from tiposessao in tG.DefaultIfEmpty()
                join utente in _context.Utentes on sessao.UtentesId equals utente.Id into uG
                from utente in uG.DefaultIfEmpty()
                join funcionario in _context.Funcionarios on sessao.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                select new
                {
                    Id = sessao.Id,
                    TipoSessaoId = sessao.TiposSessaoId,
                    TipoSessao = tiposessao.Descricao,
                    FuncionarioId = sessao.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    UtenteId = sessao.UtentesId,
                    Utente = utente.Nome,
                    Dia = sessao.Dia,
                    Observacoes = sessao.Observacoes
                }
            ).ToListAsync();

            return Ok(sessaoDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Sessao>> InserirSessao([FromBody] Sessao sessao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Sessoes.Add(sessao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterSessao), new { id = sessao.Id }, sessao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarSesao(int id, [FromBody] Sessao sessao)
        {
            if (id != sessao.Id)
            {
                return BadRequest();
            }

            _context.Entry(sessao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessaoExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverSessao(int id)
        {
            var sessao = await _context.Sessoes.FindAsync(id);
            if (sessao == null)
            {
                return NotFound();
            }

            _context.Sessoes.Remove(sessao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessaoExists(int id)
        {
            return _context.Sessoes.Any(e => e.Id == id);
        }
    }
}
