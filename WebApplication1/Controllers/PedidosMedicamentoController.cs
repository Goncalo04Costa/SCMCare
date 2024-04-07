using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PedidosMedicamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosMedicamentoController(AppDbContext context)
        {
            _context = context;
        }
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoMedicamento>>> ObterTodosPedidoMedicamento()
        {
            var pedidoMedicamento = await _context.PedidosMedicamento.ToListAsync();
            return Ok(pedidoMedicamento);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoMedicamento>> ObterPedidoMedicamento(int id)
        {
            var pedidoMedicamento = await _context.PedidosMedicamento.FindAsync(id);

            if (pedidoMedicamento = null)
            {
                return NotFound();
            }
            return Ok(pedidoMedicamento);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoMedicamento>> InserirPedidoMedicamento([FromBody] PedidoMedicamento pedidoMedicamento)
        {
            if (pedidoMedicamento == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.PedidosMedicamento.Add(pedidoMedicamento);
            await _context.SaveChangesAsync();

            return Ok("pedidoMedicamento adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaPedidoMedicamento(int id, [FromBody] PedidoMedicamento novoPedidoMedicamento)
        {
            var pedidoMedicamento = await _context.PedidosMedicamento.FindAsync(id);

            if (pedidoMedicamento == null)
            {
                return NotFound($"Não foi possível encontrar o pedidoMedicamento com o ID {id}");
            }

            pedidoMedicamento.MedicamentosId = novoPedidoMedicamento.MedicamentosId;
            pedidoMedicamento.FuncionariosId = novoPedidoMedicamento.FuncionariosId;
            pedidoMedicamento.Quantidade = novoPedidoMedicamento.Quantidade;
            pedidoMedicamento.DataPedido = novoPedidoMedicamento.DataPedido;
            pedidoMedicamento.Estado = novoPedidoMedicamento.Estado;
            pedidoMedicamento.DataConclusao = novoPedidoMedicamento.DataConclusao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o pedidoMedicamento com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePedidoMedicamento(int id)
        {
            var pedidoMedicamento = await _context.PedidosMedicamento.FindAsync(id);

            if (pedidoMedicamento == null)
            {
                return NotFound($"Não foi possível encontrar o pedidoMedicamento com o ID {id}");
            }

            _context.PedidosMedicamento.Remove(pedidoMedicamento);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o pedidoMedicamento com o ID {id}");
        }

    }
}