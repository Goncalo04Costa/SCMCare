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

        // Método para testar a inserção de um pedido de medicamento válido
        [Fact]
        public async Task InserirPedidoMed_Valido()
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
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.Equal("pedidoMedicamento adicionado com sucesso", okResult.Value);
        }

        // Método para testar a inserção de um pedido de medicamento nulo
        [Fact]
        public async Task InserirPedidoMed_Nulo()
        {
            // Act
            var result = await _controller.InserirPedidoMedicamento(null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // Método para testar a obtenção de um pedido de medicamento que exista
        [Fact]
        public async Task ObterPedidoMed_Existir()
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

            var inserirResultado = await _controller.InserirPedidoMedicamento(pedidoMedicamento);

            // Act 
            var result = await _controller.ObterPedidoMedicamento(1); // Supondo que o ID do pedido seja 1

            // Assert 
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsType<PedidoMedicamento>(okResult.Value);
        }

        // Método para testar a obtenção de um pedido de medicamento inexistente
        [Fact]
        public async Task ObterPedidoMed_Inexistente()
        {
            // Arrange - Supondo que não há pedido de medicamento com o ID 100
            var idPedidoInexistente = 100;

            // Act 
            var result = await _controller.ObterPedidoMedicamento(idPedidoInexistente);

            // Assert 
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }


        // Método para testar a atualização de um pedido de medicamento existente
        [Fact]
        public async Task AtualizarPedidoMed_Valido()
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

            var inserirResultado = await _controller.InserirPedidoMedicamento(pedidoMedicamento);

            // Modifica os dados do pedido
            pedidoMedicamento.Quantidade = 20;
            pedidoMedicamento.DataConclusao = DateTime.Now;

            // Act
            var result = await _controller.AtualizaPedidoMedicamento(1, pedidoMedicamento);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.Equal($"Foi atualizado o pedidoMedicamento com o ID {pedidoMedicamento.Id}", okResult.Value);
        }

        // Método para testar a tentativa de atualização de um pedido de medicamento inexistente
        [Fact]
        public async Task AtualizarPedidoMed_Inexistente()
        {
            // Arrange
            var pedidoMedicamento = new PedidoMedicamento
            {
                Id = 100, // Supondo que o ID 100 não existe
                MedicamentosId = 1,
                FuncionariosId = 1,
                Quantidade = 10,
                DataPedido = DateTime.Now,
                Estado = 0,
                DataConclusao = null
            };

            // Act
            var result = await _controller.AtualizaPedidoMedicamento(100, pedidoMedicamento);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // Método para testar a remoção de um pedido de medicamento existente
        [Fact]
        public async Task RemovePedidoMed_Valido()
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

            var inserirResultado = await _controller.InserirPedidoMedicamento(pedidoMedicamento);

            // Act
            var result = await _controller.RemovePedidoMedicamento(1); // Supondo que o ID do pedido seja 1

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.Equal($"Foi removido o pedidoMedicamento com o ID {pedidoMedicamento.Id}", okResult.Value);
        }
 
        // Método para testar a tentativa de remoção de um pedido de medicamento inexistente
        [Fact]
        public async Task RemovePedidoMed_Inexistente()
        {
            // Act
            var result = await _controller.RemovePedidoMedicamento(100); // Supondo que o ID do pedido inexistente é 100

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
