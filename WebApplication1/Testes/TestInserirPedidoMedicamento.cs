using Xunit;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Testes
{
    public class TestInserirPedidoMedicamento
    {
        private PedidosMedicamentoController _controller;
        private DbContextOptions<AppDbContext> _options;

        public TestInserirPedidoMedicamento()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new AppDbContext(_options))
            {

            }

            var dbContext = new AppDbContext(_options);
            _controller = new PedidosMedicamentoController(dbContext);
        }

        [Fact]
        public async Task InserirPedidoMedicamento_DeveRetornarOk_QuandoPedidoMedicamentoForValido()
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
            var result = await _controller.InserirPedidoMedicamento(pedidoMedicamento);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<OkObjectResult>(result.Result); // Verifica se o resultado é um OkObjectResult

            var okResult = result.Result as OkObjectResult;
            Assert.Equal("pedidoMedicamento adicionado com sucesso", okResult.Value); // Verifica a mensagem de sucesso
        }

        [Fact]
        public async Task InserirPedidoMedicamento_DeveRetornarBadRequest_QuandoPedidoMedicamentoForNulo()
        {
            // Arrange
            PedidoMedicamento pedidoMedicamento = null;

            // Act 
            var result = await _controller.InserirPedidoMedicamento(pedidoMedicamento);

            // Assert 
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<BadRequestObjectResult>(result.Result); // Verifica se o resultado é um BadRequestObjectResult

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("Objeto inválido", badRequestResult.Value); // Verifica a mensagem de erro
        }

        // Implemente outros métodos de teste para as operações restantes (Atualizar, Remover, ObterPorId) conforme necessário
    }
}
