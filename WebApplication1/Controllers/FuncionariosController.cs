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

        [HttpGet("{id}")]
        public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios
                //.Include(f => f.Contactos) // Inclui os contatos do funcionário
                .FirstOrDefaultAsync(f => f.FuncionarioID == id);

            if (funcionario == null)
            {
                return NotFound($"Funcionário com o ID {id} não encontrado");
            }

            return Ok(funcionario);
        }

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
