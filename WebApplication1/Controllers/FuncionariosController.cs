using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Modelos;
using Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FuncionariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> ObterTodosFuncionarios(
            int? idMin = null, int? idMax = null,
            string? nomeMin = null, string? nomeMax = null,
            int? tipoFuncionarioId = null,
            bool historico0 = false, bool historico1 = false)
        {
            IQueryable<Funcionario> query = _context.Funcionarios;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMin) >= 0);
            }

            if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMax + "ZZZ") <= 0);
            }

            if (tipoFuncionarioId.HasValue)
            {
                query = query.Where(d => d.TiposFuncionarioId == tipoFuncionarioId.Value);
            }

            if (historico0 && !historico1)
            {
                query = query.Where(d => !d.Historico);
            }
            else if (!historico0 && historico1)
            {
                query = query.Where(d => d.Historico);
            }


            var funcionarioDetalhes = await (
                from funcionario in query
                join tipoFuncionario in _context.TiposFuncionario on funcionario.TiposFuncionarioId equals tipoFuncionario.Id into tG
                from tipoFuncionario in tG.DefaultIfEmpty()
                select new
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    TiposFuncionarioId = funcionario.TiposFuncionarioId,
                    Tipo = tipoFuncionario.Descricao,
                    Historico = funcionario.Historico
                }
            ).ToListAsync();

            return Ok(funcionarioDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Funcionario>> ObterFuncionario(int id)
        {
            IQueryable<Funcionario> query = _context.Funcionarios; 
            query = query.Where(d => d.Id == id);

            var funcionarioDetalhes = await (
                from funcionario in query
                join tipoFuncionario in _context.TiposFuncionario on funcionario.TiposFuncionarioId equals tipoFuncionario.Id into tG
                from tipoFuncionario in tG.DefaultIfEmpty()
                select new
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    TiposFuncionarioId = funcionario.TiposFuncionarioId,
                    Tipo = tipoFuncionario.Descricao,
                    Historico = funcionario.Historico,
                    Contactos = _context.ContactosFuncionarios
                        .Where(cf => cf.FuncionariosId == funcionario.Id)
                        .Join(
                            _context.TipoContacto,
                            cf => cf.TipoContactoId,
                            tc => tc.Id,
                            (cf, tc) => new
                            {
                                TipoContactoId = tc.Id,
                                TipoContacto = tc.Descricao,
                                Valor = cf.Valor
                            }
                        )
                        .ToList()
                }
            ).ToListAsync();

            return Ok(funcionarioDetalhes);
        }

        [HttpPost]
        public async Task<ActionResult<Funcionario>> InserirFuncionario([FromBody] Funcionario funcionario)
        {
            if (funcionario == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            // !!! Adaptar código para depois guardar os contactos ao inserir o funcionario
            // Guarda os contatos do funcionário
            //if (funcionario.Contactos != null && funcionario.Contactos.Any())
            //{
            //    foreach (var contacto in funcionario.Contactos)
            //    {
            //        contacto.FuncionariosId = funcionario.Id;
            //        _context.ContactosFuncionarios.Add(contacto);
            //    }
            //    await _context.SaveChangesAsync();
            //}

            return Ok("Funcionário adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarFuncionario(int id, [FromBody] Funcionario novoFuncionario)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null)
            {
                return NotFound($"Não foi possível encontrar o funcionário com o ID {id}");
            }

            funcionario.Nome = novoFuncionario.Nome;
            funcionario.TiposFuncionarioId = novoFuncionario.TiposFuncionarioId;
            funcionario.Historico = novoFuncionario.Historico;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Funcionário atualizado com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null)
            {
                return NotFound($"Funcionário com o ID {id} não encontrado");
            }

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return Ok($"Funcionário com o ID {id} removido com sucesso");
        }

    }
}
