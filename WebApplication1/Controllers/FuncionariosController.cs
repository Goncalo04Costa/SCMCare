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
    public class FuncionariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> ObterTodosFuncionarios()
        {
            var funcionarios = await _context.Funcionarios.ToListAsync();
            return Ok(funcionarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Funcionario>> ObterFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return Ok(funcionario);
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
