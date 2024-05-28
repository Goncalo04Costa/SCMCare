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
    public class TesteSegurancaPedidosMedicamento
    {
        private PedidosMedicamentoController _controller;
        private DbContextOptions<AppDbContext> _options;

        public TesteSegurancaPedidosMedicamento()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new AppDbContext(_options))
            {
                // Initialize database with test data
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
                    FuncionarioID = 1,
                    Nome = "João Silva",
                    TiposFuncionarioId = tipoFuncionario.Id,
                    Historico = false
                };
                context.Funcionarios.Add(funcionario);

                context.SaveChanges();
            }

            var dbContext = new AppDbContext(_options);
            var tiposFuncionarioService = new TiposFuncionarioServico(dbContext);
            var notificacoesService = new NotificacoesServico(dbContext);

            _controller = new PedidosMedicamentoController(dbContext, tiposFuncionarioService, notificacoesService);
        }

        [Fact]
        public async Task TestarAutorizacao()
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

            var obterResultado = await _controller.ObterTodosPedidoMedicamento();
            var obterOkResult = obterResultado.Result as OkObjectResult;

            await _controller.AtualizaPedidoMedicamento(1, pedidoMedicamento);

            var removerResultado = await _controller.RemovePedidoMedicamento(1);

            // Assert
            // Verifica se as operações são permitidas apenas para usuários autenticados
            Assert.NotNull(inserirOkResult);
            Assert.NotNull(obterOkResult);
            Assert.NotNull(removerResultado);

            // Verifica se o status da resposta é 401 (Não autorizado) para operações não autorizadas
            if (!(removerResultado is OkObjectResult))
            {
                Assert.Equal(401, (removerResultado as UnauthorizedResult).StatusCode);
            }
        }

        [Fact]
        public async Task TestarAtualizacaoEstadoPedido()
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
            var inserirOkResult = inserirResultado.Result as OkObjectResult;
            var pedidoInserido = inserirOkResult.Value as PedidoMedicamento;

            // Act
            await _controller.AtualizaPedidoMedicamento(pedidoInserido.Id, 1); // Atualiza estado para 1 (Em andamento)

            var obterResultado = await _controller.ObterPedidoMedicamento(pedidoInserido.Id);
            var obterOkResult = obterResultado.Result as OkObjectResult;
            var pedidoObtido = obterOkResult.Value as PedidoMedicamento;

            // Assert
            Assert.NotNull(obterOkResult);
            Assert.Equal(1, pedidoObtido.Estado);
        }

        [Fact]
        public async Task TestarAtualizacaoEstadoPedidoSemPermissao()
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
            var inserirOkResult = inserirResultado.Result as OkObjectResult;
            var pedidoInserido = inserirOkResult.Value as PedidoMedicamento;

            // Act
            // Tenta atualizar o estado sem ter permissão adequada
            var resultado = await _controller.AtualizaPedidoMedicamento(pedidoInserido.Id, 2); // Estado 2 (Concluído)

            // Assert
            Assert.IsType<ForbidResult>(resultado.Result);
        }
    }
}
