using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Linq;
using System.Threading.Tasks;

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
            bool historico0 = false, bool historico1 = false)
        {
            IQueryable<Funcionario> query = _context.Funcionarios;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.FuncionarioID >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.FuncionarioID <= idMax.Value);
            }

            if (!string.IsNullOrEmpty(nomeMin))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMin) >= 0);
            }

            if (!string.IsNullOrEmpty(nomeMax))
            {
                query = query.Where(d => d.Nome.CompareTo(nomeMax + "ZZZ") <= 0);
            }

            if (historico0 && !historico1)
            {
                query = query.Where(d => !d.Historico);
            }

            else if (!historico0 && historico1)
            {
                query = query.Where(d => d.Historico);
            }

            var dados = await query.ToListAsync();
            return Ok(dados);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios
                
                .FirstOrDefaultAsync(f => f.FuncionarioID == id);

            if (funcionario == null)
            {
                return NotFound($"Funcionário com o ID {id} não encontrado");
            }

            return Ok(funcionario);
        }


        //Metodo para inserir um novo funcnionario
        [HttpPost]
        public async Task<ActionResult<Funcionario>> PostFuncionario([FromBody] Funcionario funcionario)
        {
            if (funcionario == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.FuncionarioID }, funcionario);
        }


        //Metodo para apresentar um funcionario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuncionario(int id, [FromBody] Funcionario funcionarioAtualizado)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null)
            {
                return NotFound($"Funcionário com o ID {id} não encontrado");
            }

            funcionario.Nome = funcionarioAtualizado.Nome;
            funcionario.TiposFuncionarioId = funcionarioAtualizado.TiposFuncionarioId;
            funcionario.Historico = funcionarioAtualizado.Historico;

            await _context.SaveChangesAsync();

            return Ok($"Funcionário com o ID {id} atualizado com sucesso");
        }

        //Metodo para remover um funcionario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
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
