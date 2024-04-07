using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PedidosMaterialController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosMaterialController(AppDbContext context)
        {
            _context = context;
        }
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoMaterial>>> ObterTodosPedidoMaterial()
        {
            var pedidoMaterial = await _context.PedidosMaterial.ToListAsync();
            return Ok(pedidoMaterial);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoMaterial>> ObterPedidoMaterial(int id)
        {
            var pedidoMaterial = await _context.PedidosMaterial.FindAsync(id);

            if (pedidoMaterial = null)
            {
                return NotFound();
            }
            return Ok(pedidoMaterial);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoMaterial>> InserirPedidoMaterial([FromBody] PedidoMaterial pedidoMaterial)
        {
            if (pedidoMaterial == null)
            {
                return BadRequest("Objeto inválido");
            }

            _context.PedidosMaterial.Add(pedidoMaterial);
            await _context.SaveChangesAsync();

            return Ok("PedidoMaterial adicionado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaPedidoMaterial(int id, [FromBody] PedidoMaterial novoPedidoMaterial)
        {
            var pedidoMaterial = await _context.PedidosMaterial.FindAsync(id);

            if (pedidoMaterial == null)
            {
                return NotFound($"Não foi possível encontrar o pedidoMaterial com o ID {id}");
            }

            pedidoMaterial.MateriaisId = novoPedidoMaterial.MateriaisId;
            pedidoMaterial.FuncionariosId = novoPedidoMaterial.FuncionariosId;
            pedidoMaterial.QuantidadeTotal = novoPedidoMaterial.QuantidadeTotal;
            pedidoMaterial.DataPedido = novoPedidoMaterial.DataPedido;
            pedidoMaterial.Estado = novoPedidoMaterial.Estado;
            pedidoMaterial.DataConclusao = novoPedidoMaterial.DataConclusao;

            try
            {
                await _context.SaveChangesAsync();

                return Ok($"Foi atualizado o pedidoMaterial com o ID {id}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePedidoMaterial(int id)
        {
            var pedidoMaterial = await _context.PedidosMaterial.FindAsync(id);

            if (pedidoMaterial == null)
            {
                return NotFound($"Não foi possível encontrar o pedidoMaterial com o ID {id}");
            }

            _context.PedidosMaterial.Remove(pedidoMaterial);
            await _context.SaveChangesAsync();

            return Ok($"Foi removido o pedidoMaterial com o ID {id}");
        }

    }
}