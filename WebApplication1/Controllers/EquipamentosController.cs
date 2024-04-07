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
    public class EquipamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipamentosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipamento>>> ObterTodosEquipamentos()
        {
            var equipamentos = await _context.Equipamentos.ToListAsync();
            return Ok(equipamentos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Equipamento>> ObterEquipamento(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
            {
                return NotFound();
            }

            return Ok(equipamento);
        }

        [HttpPost]
        public async Task<ActionResult<Equipamento>> InserirEquipamento([FromBody] Equipamento equipamento)
        {
            if (equipamento == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.Equipamentos.Add(equipamento);
            await _context.SaveChangesAsync();

            return Ok("Equipamento adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEquipamento(int id, [FromBody] Equipamento novoEquipamento)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
            {
                return NotFound($"Não foi possível encontrar o equipamento com o ID {id}");
            }

            equipamento.Descricao = novoEquipamento.Descricao;
            equipamento.Historico = novoEquipamento.Historico;
            equipamento.TiposEquipamentoId = novoEquipamento.TiposEquipamentoId;
            equipamento.QuartosId = novoEquipamento.QuartosId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Equipamento atualizado com sucesso para o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverEquipamento(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if (equipamento == null)
            {
                return NotFound($"Equipamento com o ID {id} não encontrado");
            }

            _context.Equipamentos.Remove(equipamento);
            await _context.SaveChangesAsync();

            return Ok($"Equipamento com o ID {id} removido com sucesso");
        }
    }
}
