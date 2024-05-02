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
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoMedicamento>>> ObterTodosPedidoMedicamento(
            int? idMin = null, int? idMax = null,
            int? medicamentoId = null,
            int? funcionarioId = null,
            int? quantidadeMin = null, int? quantidadeMax = null,
            DateTime? dataMin = null, DateTime? dataMax = null,
            int? estado = null,
            DateTime? dataConclusaoMin = null, DateTime? dataConclusaoMax = null)
        {
            IQueryable<PedidoMedicamento> query = _context.PedidosMedicamento;

            if (idMin.HasValue)
            {
                query = query.Where(d => d.Id >= idMin.Value);
            }

            if (idMax.HasValue)
            {
                query = query.Where(d => d.Id <= idMax.Value);
            }

            if (medicamentoId.HasValue)
            {
                query = query.Where(d => d.MedicamentosId == medicamentoId.Value);
            }

            if (funcionarioId.HasValue)
            {
                query = query.Where(d => d.FuncionariosId == funcionarioId.Value);
            }

            if (quantidadeMin.HasValue)
            {
                query = query.Where(d => d.Quantidade >= quantidadeMin.Value);
            }

            if (quantidadeMax.HasValue)
            {
                query = query.Where(d => d.Quantidade <= quantidadeMax.Value);
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
                join medicamento in _context.Materiais on pedido.MedicamentosId equals medicamento.Id into mG
                from medicamento in mG.DefaultIfEmpty()
                select new
                {
                    Id = pedido.Id,
                    MedicamentoId = pedido.MedicamentosId,
                    Medicamento = medicamento.Nome,
                    FuncionarioId = pedido.FuncionariosId,
                    Funcionario = funcionario.Nome,
                    QuantidadePedido = pedido.Quantidade,
                    DataPedido = pedido.DataPedido,
                    Estado = pedido.Estado,
                    DataConclusao = pedido.DataConclusao
                }
            ).ToListAsync();

            return Ok(pedidosDetalhes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoMedicamento>> ObterPedidoMedicamento(int id)
        {
            var pedidoMedicamento = await _context.PedidosMedicamento.FindAsync(id);

            if (pedidoMedicamento == null)
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