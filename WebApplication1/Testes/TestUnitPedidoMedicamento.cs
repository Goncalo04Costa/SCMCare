using Xunit;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication1.Servicos;
using System.Linq;

namespace WebApplication1.Testes
{
    public class TestUnitPedidoMedicamento
    {
        private DbContextOptions<AppDbContext> _options;

        public TestUnitPedidoMedicamento()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for each test
                .Options;
        }

        // Method to reset the database to a known state before each test
        private async Task ResetDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Insert initial data as needed
                var tipoFuncionario = new TipoFuncionario
                {
                    Descricao = "Diretor(a)"
                };
                context.TiposFuncionario.Add(tipoFuncionario);

                var medicamento = new Medicamento
                {
                    Id = 1,
                    Nome = "Paracetamol",
                    Descricao = "Medicamento para dor e febre",
                    Limite = 100,
                    Ativo = true
                };
                context.Medicamentos.Add(medicamento);

                var funcionario = new Funcionario
                {
                    Nome = "João Silva",
                    TiposFuncionarioId = tipoFuncionario.Id,
                    Historico = false
                };
                context.Funcionarios.Add(funcionario);

                await context.SaveChangesAsync();
            }
        }

        // Método para testar a inserção de um pedido de medicamento válido
        [Fact]
        public async Task InserirPedidoMed_Valido()
        {
            // Arrange
            await ResetDatabase();

            using (var context = new AppDbContext(_options))
            {
                var tiposFuncionarioService = new TiposFuncionarioServico(context);
                var notificacoesService = new NotificacoesServico(context);

                var controller = new PedidosMedicamentoController(context, tiposFuncionarioService, notificacoesService);

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
                var result = await controller.InserirPedidoMedicamento(pedidoMedicamento);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result.Result);

                var okResult = result.Result as OkObjectResult;
                Assert.Equal("Pedido de medicamentos e notificação adicionados com sucesso.", okResult.Value);
            }
        }

        // Método para testar a inserção de um pedido de medicamento nulo
        [Fact]
        public async Task InserirPedidoMed_Nulo()
        {
            // Arrange
            await ResetDatabase();

            using (var context = new AppDbContext(_options))
            {
                var tiposFuncionarioService = new TiposFuncionarioServico(context);
                var notificacoesService = new NotificacoesServico(context);

                var controller = new PedidosMedicamentoController(context, tiposFuncionarioService, notificacoesService);

                // Act
                var result = await controller.InserirPedidoMedicamento(null);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }
        }

        // Método para testar a obtenção de um pedido de medicamento que exista
        [Fact]
        public async Task ObterPedidoMed_Existir()
        {
            // Arrange
            await ResetDatabase();

            using (var context = new AppDbContext(_options))
            {
                var tiposFuncionarioService = new TiposFuncionarioServico(context);
                var notificacoesService = new NotificacoesServico(context);

                var controller = new PedidosMedicamentoController(context, tiposFuncionarioService, notificacoesService);

                var pedidoMedicamento = new PedidoMedicamento
                {
                    MedicamentosId = 1,
                    FuncionariosId = 1,
                    Quantidade = 10,
                    DataPedido = DateTime.Now,
                    Estado = 0,
                    DataConclusao = null
                };

                var inserirResultado = await controller.InserirPedidoMedicamento(pedidoMedicamento);

                // Act 
                var result = await controller.ObterPedidoMedicamento(1); // Supondo que o ID do pedido seja 1

                // Assert 
                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result.Result);

                var okResult = result.Result as OkObjectResult;
                Assert.IsType<PedidoMedicamento>(okResult.Value);
            }
        }

        // Método para testar a obtenção de um pedido de medicamento inexistente
        [Fact]
        public async Task ObterPedidoMed_Inexistente()
        {
            // Arrange
            await ResetDatabase();

            using (var context = new AppDbContext(_options))
            {
                var tiposFuncionarioService = new TiposFuncionarioServico(context);
                var notificacoesService = new NotificacoesServico(context);

                var controller = new PedidosMedicamentoController(context, tiposFuncionarioService, notificacoesService);

                var idPedidoInexistente = 100;

                // Act 
                var result = await controller.ObterPedidoMedicamento(idPedidoInexistente);

                // Assert 
                Assert.NotNull(result);
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        // Método para testar a atualização de um pedido de medicamento existente
        [Fact]
        public async Task AtualizarPedidoMed_Valido()
        {
            // Arrange
            await ResetDatabase();

            using (var context = new AppDbContext(_options))
            {
                var tiposFuncionarioService = new TiposFuncionarioServico(context);
                var notificacoesService = new NotificacoesServico(context);

                var controller = new PedidosMedicamentoController(context, tiposFuncionarioService, notificacoesService);

                var pedidoMedicamento = new PedidoMedicamento
                {
                    MedicamentosId = 1,
                    FuncionariosId = 1,
                    Quantidade = 10,
                    DataPedido = DateTime.Now,
                    Estado = 0,
                    DataConclusao = null
                };

                var inserirResultado = await controller.InserirPedidoMedicamento(pedidoMedicamento);

                var pedidoMedicamento2 = new PedidoMedicamento
                {
                    Id = 1,
                    MedicamentosId = 1,
                    FuncionariosId = 1,
                    Quantidade = 20,
                    DataPedido = DateTime.Now,
                    Estado = 0,
                    DataConclusao = DateTime.Now
                };

                // Act
                var result = await controller.AtualizaPedidoMedicamento(1, pedidoMedicamento2);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result);

                var okResult = result as OkObjectResult;
                Assert.Equal($"Foi atualizado o pedidoMedicamento com o ID {pedidoMedicamento.Id}", okResult.Value);
            }
        }

        // Método para testar a tentativa de atualização de um pedido de medicamento inexistente
        [Fact]
        public async Task AtualizarPedidoMed_Inexistente()
        {
            // Arrange
            await ResetDatabase();

            using (var context = new AppDbContext(_options))
            {
                var tiposFuncionarioService = new TiposFuncionarioServico(context);
                var notificacoesService = new NotificacoesServico(context);

                var controller = new PedidosMedicamentoController(context, tiposFuncionarioService, notificacoesService);

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
                var result = await controller.AtualizaPedidoMedicamento(100, pedidoMedicamento);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<NotFoundObjectResult>(result);
            }
        }

        // Método para testar a remoção de um pedido de medicamento existente
        [Fact]
        public async Task RemovePedidoMed_Valido()
        {
            // Arrange
            await ResetDatabase();

            using (var context = new AppDbContext(_options))
            {
                var tiposFuncionarioService = new TiposFuncionarioServico(context);
                var notificacoesService = new NotificacoesServico(context);

                var controller = new PedidosMedicamentoController(context, tiposFuncionarioService, notificacoesService);

                var pedidoMedicamento = new PedidoMedicamento
                {
                    MedicamentosId = 1,
                    FuncionariosId = 1,
                    Quantidade = 10,
                    DataPedido = DateTime.Now,
                    Estado = 0,
                    DataConclusao = null
                };

                var inserirResultado = await controller.InserirPedidoMedicamento(pedidoMedicamento);

                // Act
                var result = await controller.RemovePedidoMedicamento(1); // Supondo que o ID do pedido
            }
        }
    }
}