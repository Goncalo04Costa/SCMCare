using Xunit;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebApplication1.Servicos;

namespace WebApplication1.Testes
{
    public class TesteInserirPedidoMedicamento
    {
        private readonly PedidosMedicamentoController _controller;

        public TesteInserirPedidoMedicamento()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            var dbContext = new AppDbContext(options);

            // Instantiate TiposFuncionarioServico with the required AppDbContext parameter
            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);

            // Instantiate NotificacoesServico with the required AppDbContext parameter
            var notificacoesService = new NotificacoesServico(dbContext);

            _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);
        }

        [Fact]
        public async Task TestarOperacoesCRUD()
        {
            // Arrange
            var pedidoMedicamento = new PedidoMedicamento
            {
                MedicamentosId = 1,
                FuncionariosId = 1,
                Quantidade = 10,
                DataPedido = DateTime.Now,
                Estado = 0,
                DataConclusao = null
            };

            // Act
            var inserirResultado = await _controller.InserirPedidoMedicamento(pedidoMedicamento);
            var inserirOkResult = inserirResultado.Result as OkObjectResult;
            var pedidoMedicamentoInserido = inserirOkResult.Value as PedidoMedicamento;

            var obterResultado = await _controller.ObterPedidoMedicamento(pedidoMedicamentoInserido.Id);
            var obterOkResult = obterResultado.Result as OkObjectResult;
            var pedidoMedicamentoObtido = obterOkResult.Value as PedidoMedicamento;

            // Assert
            Assert.NotNull(inserirOkResult);
            Assert.NotNull(pedidoMedicamentoInserido);
            Assert.Equal(pedidoMedicamento.MedicamentosId, pedidoMedicamentoInserido.MedicamentosId);
            Assert.Equal(pedidoMedicamento.FuncionariosId, pedidoMedicamentoInserido.FuncionariosId);
            Assert.Equal(pedidoMedicamento.Quantidade, pedidoMedicamentoInserido.Quantidade);

            Assert.NotNull(obterOkResult);
            Assert.NotNull(pedidoMedicamentoObtido);
            Assert.Equal(pedidoMedicamentoInserido.Id, pedidoMedicamentoObtido.Id);

            // Modificar e atualizar
            pedidoMedicamentoInserido.Quantidade = 20;
            pedidoMedicamentoInserido.DataConclusao = DateTime.Now;

            var atualizarResultado = await _controller.AtualizaPedidoMedicamento(pedidoMedicamentoInserido.Id, pedidoMedicamentoInserido);
            var atualizarOkResult = atualizarResultado as OkObjectResult;
            Assert.NotNull(atualizarOkResult);
            Assert.Equal($"Foi atualizado o pedidoMedicamento com o ID {pedidoMedicamentoInserido.Id}", atualizarOkResult.Value);

            // Remover
            var removerResultado = await _controller.RemovePedidoMedicamento(pedidoMedicamentoInserido.Id);
            var removerOkResult = removerResultado as OkObjectResult;
            Assert.NotNull(removerOkResult);
            Assert.Equal($"Foi removido o pedidoMedicamento com o ID {pedidoMedicamentoInserido.Id}", removerOkResult.Value);
        }
    }
}
