using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoMaterial>>> ObterTodosPedidoMaterial(
            int? idMin = null, int? idMax = null,
            int? materialId = null, 
            int? funcionarioId = null,
            int? quantidadeMin = null, int? quantidadeMax = null,
            DateTime? dataMin = null, DateTime? dataMax = null,
            int? estado = null,
            DateTime? dataConclusaoMin = null, DateTime? dataConclusaoMax = null)
        {
            IQueryable<PedidoMaterial> query = _context.PedidosMaterial;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (materialId.HasValue)
            {
                query = query.Where(d => d.MateriaisId == materialId.Value);
            }

            if (funcionarioId.HasValue)
            {
                query = query.Where(d => d.FuncionariosId == funcionarioId.Value);
            }

            if (quantidadeMin.HasValue)
            {
                query = query.Where(d => d.QuantidadeTotal >= quantidadeMin.Value);
            }

            if (quantidadeMax.HasValue)
            {
                query = query.Where(d => d.QuantidadeTotal <= quantidadeMax.Value);
            }

            if (dataMin.HasValue)
            {
                query = query.Where(d => d.DataPedido >= dataMin.Value);
            }

            if (dataMax.HasValue)
            {
                query = query.Where(d => d.DataPedido <= dataMax.Value);
            }

            if (estado.HasValue)
            {
                query = query.Where(d => d.Estado == estado.Value);
            }

            if (dataConclusaoMin.HasValue)
            {
                query = query.Where(d => d.DataConclusao >= dataConclusaoMin.Value);
            }

            if (dataConclusaoMax.HasValue)
            {
                query = query.Where(d => d.DataConclusao <= dataConclusaoMax.Value);
            }


            var pedidosDetalhes = await (
                from pedido in query
                join funcionario in _context.Funcionarios on pedido.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join material in _context.Materiais on pedido.MateriaisId equals material.Id into mG
                from material in mG.DefaultIfEmpty()
                select new
                {
                    Id = pedido.Id,
                    MaterialId = pedido.MateriaisId,
                    Material = material.Nome,
                    FuncionarioId = pedido.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    QuantidadePedido = pedido.QuantidadeTotal,
                    DataPedido = pedido.DataPedido,
                    Estado = pedido.Estado,
                    DataConclusao = pedido.DataConclusao
                }
            ).ToListAsync();

            return Ok(pedidosDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoMaterial>> ObterPedidoMaterial(int id)
        {
            IQueryable<PedidoMaterial> query = _context.PedidosMaterial;
            query = query.Where(d => d.Id == id);

            var pedidoDetalhes = await (
                from pedido in query
                join funcionario in _context.Funcionarios on pedido.FuncionariosId equals funcionario.FuncionarioID into fG
                from funcionario in fG.DefaultIfEmpty()
                join material in _context.Materiais on pedido.MateriaisId equals material.Id into mG
                from material in mG.DefaultIfEmpty()
                select new
                {
                    Id = pedido.Id,
                    MaterialId = pedido.MateriaisId,
                    Material = material.Nome,
                    FuncionarioId = pedido.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    QuantidadePedido = pedido.QuantidadeTotal,
                    DataPedido = pedido.DataPedido,
                    Estado = pedido.Estado,
                    DataConclusao = pedido.DataConclusao
                }
            ).ToListAsync();

            return Ok(pedidoDetalhes);
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