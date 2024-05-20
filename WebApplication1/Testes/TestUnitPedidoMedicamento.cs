using Xunit;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication1.Servicos;

namespace WebApplication1.Testes
{
    public class TestUnitPedidoMedicamento
    {
        //private PedidosMedicamentoController _controller;
        private DbContextOptions<AppDbContext> _options;

        public TestUnitPedidoMedicamento()
        {
            //_options = new DbContextOptionsBuilder<AppDbContext>()
            //    .UseInMemoryDatabase(databaseName: "test_database")
            //    .Options;

            //var dbContext = new AppDbContext(_options);

            //var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            //var notificacoesService = new NotificacoesServico(dbContext);

            //_controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);

        }

        // Método para testar a inserção de um pedido de medicamento válido
        [Fact]
        public async Task InserirPedidoMed_Valido()
        {
            // Arrange
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "test_database1").Options;
            var dbContext = new AppDbContext(_options);

            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            var notificacoesService = new NotificacoesServico(dbContext);

            var _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);

            // Inserir tipo de funcionário "Diretor(a)"
            var tipoFuncionario = new TipoFuncionario
            {
                Descricao = "Diretor(a)"
            };
            dbContext.TiposFuncionario.Add(tipoFuncionario);
            await dbContext.SaveChangesAsync();

            // Inserir medicamento 1
            var medicamento = new Medicamento
            {
                Nome = "Paracetamol",
                Descricao = "Medicamento para dor e febre",
                Limite = 100,
                Ativo = true
            };
            dbContext.Medicamentos.Add(medicamento);
            await dbContext.SaveChangesAsync();

            // Inserir funcionário
            var funcionario = new Funcionario
            {
                Nome = "João Silva",
                TiposFuncionarioId = tipoFuncionario.Id,
                Historico = false
            };
            dbContext.Funcionarios.Add(funcionario);
            await dbContext.SaveChangesAsync();

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
            Assert.Equal("Pedido de medicamentos e notificação adicionados com sucesso.", okResult.Value);
        }

        // Método para testar a inserção de um pedido de medicamento nulo
        [Fact]
        public async Task InserirPedidoMed_Nulo()
        {
            // Arrange
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "test_database2").Options;
            var dbContext = new AppDbContext(_options);

            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            var notificacoesService = new NotificacoesServico(dbContext);

            var _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);

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
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "test_database3").Options;
            var dbContext = new AppDbContext(_options);

            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            var notificacoesService = new NotificacoesServico(dbContext);

            var _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);

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
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "test_database4").Options;
            var dbContext = new AppDbContext(_options);

            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            var notificacoesService = new NotificacoesServico(dbContext);

            var _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);

            var idPedidoInexistente = 100;

            // Act 
            var result = await _controller.ObterPedidoMedicamento(idPedidoInexistente);

            // Assert 
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }


        // Método para testar a atualização de um pedido de medicamento existente
        [Fact]
        public async Task AtualizarPedidoMed_Valido()
        {
            // Arrange
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "test_database5").Options;
            var dbContext = new AppDbContext(_options);

            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            var notificacoesService = new NotificacoesServico(dbContext);

            var _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);

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

            var pedidoMedicamento2 = new PedidoMedicamento
            {
                MedicamentosId = 1,
                FuncionariosId = 1,
                Quantidade = 20,
                DataPedido = DateTime.Now,
                Estado = 0,
                DataConclusao = DateTime.Now
            };

            //// Modifica os dados do pedido
            //pedidoMedicamento2.Quantidade = 20;
            //pedidoMedicamento2.DataConclusao = DateTime.Now;

            // Act
            var result = await _controller.AtualizaPedidoMedicamento(1, pedidoMedicamento2);

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
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "test_database6").Options;
            var dbContext = new AppDbContext(_options);

            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            var notificacoesService = new NotificacoesServico(dbContext);

            var _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);

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
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "test_database7").Options;
            var dbContext = new AppDbContext(_options);

            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            var notificacoesService = new NotificacoesServico(dbContext);

            var _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);

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
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "test_database8").Options;
            var dbContext = new AppDbContext(_options);

            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            var notificacoesService = new NotificacoesServico(dbContext);

            var _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);

            var result = await _controller.RemovePedidoMedicamento(100); // Supondo que o ID do pedido inexistente é 100

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
