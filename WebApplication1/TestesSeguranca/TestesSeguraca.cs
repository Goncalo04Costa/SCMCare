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

            var atualizarResultado = await _controller.AtualizaPedidoMedicamento(1, pedidoMedicamento);
            var atualizarOkResult = atualizarResultado as OkObjectResult;

            var removerResultado = await _controller.RemovePedidoMedicamento(1);
            var removerOkResult = removerResultado as OkObjectResult;

            // Assert
            // Check if operations are allowed only for authenticated users
            Assert.NotNull(inserirOkResult);
            Assert.NotNull(obterOkResult);
            Assert.NotNull(atualizarOkResult);
            Assert.NotNull(removerOkResult);

            // Check if the response status is 401 (Unauthorized) for unauthorized operations
            //Assert.Equal(401, (obterResultado.Result as UnauthorizedResult).StatusCode);
            Assert.Equal(401, (atualizarResultado as UnauthorizedResult).StatusCode);
            Assert.Equal(401, (removerResultado as UnauthorizedResult).StatusCode);
        }
    }
}
