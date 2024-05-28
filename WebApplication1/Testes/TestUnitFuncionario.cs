using Xunit;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Testes
{
    public class FuncionariosControllerTests
    {
        private FuncionariosController _controller;
        private DbContextOptions<AppDbContext> _options;

        public FuncionariosControllerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new AppDbContext(_options))
            {

            }

            var dbContext = new AppDbContext(_options);
            _controller = new FuncionariosController(dbContext);
        }


        // -------------- Testes para verificar o comportamento dos métodos HTTP (GET, POST, PUT, DELETE). 

        // Método para testar se um funcionário pode ser inserido com sucesso (POST)
        [Fact]
        public async Task TestInserirFuncionarioValido()
        {
            // Arrange 
            var funcionario = new Funcionario { Nome = "João", TiposFuncionarioId = 1, Historico = false };

            // Act 
            var result = await _controller.PostFuncionario(funcionario);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<ActionResult<Funcionario>>(result); // Verifica se o resultado é do tipo OkObjectResult
        }

        // Método para testar se é possível obter um funcionário existente através do ID (GET)
        [Fact]
        public async Task TestGetFuncionarioIdValido()
        {
            // Arrange
            var funcionario = new Funcionario { Nome = "João", TiposFuncionarioId = 1, Historico = false };
            var resultInserir = await _controller.PostFuncionario(funcionario);
            var funcionarioInserido = (resultInserir.Result as CreatedAtActionResult).Value as Funcionario;

            // Act
            var result = await _controller.GetFuncionario(funcionarioInserido.FuncionarioID);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<OkObjectResult>(result.Result); // Verifica se o resultado é do tipo OkObjectResult

            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult.Value); // Verifica se o valor do resultado não é nulo
            Assert.IsType<Funcionario>(okResult.Value); // Verifica se o valor é do tipo Funcionario

            var funcionarioRetornado = okResult.Value as Funcionario;
            Assert.Equal(funcionarioInserido.FuncionarioID, funcionarioRetornado.FuncionarioID); // Verifica se os IDs são iguais
            Assert.Equal(funcionarioInserido.Nome, funcionarioRetornado.Nome); // Verifica se os nomes são iguais
            Assert.Equal(funcionarioInserido.Historico, funcionarioRetornado.Historico); // Verifica se os históricos são iguais
        }


        // Método para testar se um funcionário existente pode ser atualizado com sucesso (PUT)
        [Fact]
        public async Task TestAtualizarFuncionarioExistente()
        {
            // Arrange
            var funcionario = new Funcionario { Nome = "João", Historico = false };
            var resultInserir = await _controller.PostFuncionario(funcionario);
            var funcionarioInserido = (resultInserir.Result as CreatedAtActionResult).Value as Funcionario;

            var funcionarioAtualizado = new Funcionario { FuncionarioID = funcionarioInserido.FuncionarioID, Nome = "Novo Nome", Historico = true };

            // Act
            var result = await _controller.PutFuncionario(funcionarioInserido.FuncionarioID, funcionarioAtualizado);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<OkObjectResult>(result); // Verifica se o resultado é do tipo OkObjectResult
        }

        // Método para testar se um funcionário existente pode ser removido com sucesso (DELETE)
        [Fact]
        public async Task TestRemoverFuncionarioExistente()
        {
            // Arrange
            var funcionario = new Funcionario { Nome = "João", Historico = false };
            var resultInserir = await _controller.PostFuncionario(funcionario);
            var funcionarioInserido = (resultInserir.Result as CreatedAtActionResult).Value as Funcionario;

            // Act
            var result = await _controller.DeleteFuncionario(funcionarioInserido.FuncionarioID);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<OkObjectResult>(result); // Verifica se o resultado é do tipo OkObjectResult
        }

    }
}
