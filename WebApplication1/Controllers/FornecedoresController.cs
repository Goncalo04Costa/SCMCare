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
    public class FornecedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FornecedoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> ObterTodosFornecedores()
        {
            var fornecedores = await _context.Fornecedores.ToListAsync();
            return Ok(fornecedores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> ObterFornecedor(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<ActionResult<Fornecedor>> InserirFornecedor([FromBody] Fornecedor fornecedor)
        {
            if (fornecedor == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            return Ok("Fornecedor adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarFornecedor(int id, [FromBody] Fornecedor novoFornecedor)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if (fornecedor == null)
            {
                return NotFound($"Não foi possível encontrar o fornecedor com o ID {id}");
            }

            fornecedor.Nome = novoFornecedor.Nome;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Fornecedor atualizado com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverFornecedor(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if (fornecedor == null)
            {
                return NotFound($"Fornecedor com o ID {id} não encontrado");
            }

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return Ok($"Fornecedor com o ID {id} removido com sucesso");
        }
    }
}
