using Xunit;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication1.Testes
{
    public class FuncionarioControllerTests
    {
        private FuncionariosController _controller;
        private AppDbContext _dbContext;

        public FuncionarioControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            _dbContext = new AppDbContext(options);

            _controller = new FuncionariosController(_dbContext);
        }

        // -------------- Testes de Integridade --------------

        // Método para testar a integridade dos dados após inserir um funcionário
        [Fact]
        public async Task TestInserirFuncionarioIntegridadeDados()
        {
            // Arrange 
            var funcionario = new Funcionario { Nome = "João", TiposFuncionarioId = 1, Historico = false };

            // Act 
            var result = await _controller.PostFuncionario(funcionario);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<Funcionario>>(result);

            // Verificar integridade dos dados
            var funcionarioInserido = (result.Result as CreatedAtActionResult).Value as Funcionario;
            var funcionarioNabd = await _dbContext.Funcionarios.FindAsync(funcionarioInserido.FuncionarioID);

            Assert.NotNull(funcionarioNabd);
            Assert.Equal(funcionario.Nome, funcionarioNabd.Nome);
            Assert.Equal(funcionario.TiposFuncionarioId, funcionarioNabd.TiposFuncionarioId);
            Assert.Equal(funcionario.Historico, funcionarioNabd.Historico);
        }

        // Método para testar a integridade do comportamento após inserir um funcionário
        [Fact]
        public async Task TestInserirFuncionarioIntegridadeComportamento()
        {
            // Arrange 
            var funcionario = new Funcionario { Nome = "João", TiposFuncionarioId = 1, Historico = false };
            var totalFuncionariosAntes = await _dbContext.Funcionarios.CountAsync();

            // Act 
            var result = await _controller.PostFuncionario(funcionario);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<Funcionario>>(result);

            // Verificar integridade de comportamento
            var totalFuncionariosDepois = await _dbContext.Funcionarios.CountAsync();
            Assert.Equal(totalFuncionariosAntes + 1, totalFuncionariosDepois);
        }

        // Método para testar a obtenção de um funcionário que não existe
        [Fact]
        public async Task TestObterFuncionarioInexistente()
        {
            // Arrange
            int funcionarioID = 999; // ID que não existe

            // Act
            var result = await _controller.GetFuncionario(funcionarioID);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // Método para testar a atualização de um funcionário que não existe
        [Fact]
        public async Task TestAtualizarFuncionarioInexistente()
        {
            // Arrange
            int funcionarioID = 999; // ID que não existe
            var funcionario = new Funcionario { FuncionarioID = funcionarioID, Nome = "Novo Nome", TiposFuncionarioId = 1, Historico = true };

            // Act
            var result = await _controller.PutFuncionario(funcionarioID, funcionario);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Método para testar a exclusão de um funcionário que não existe
        [Fact]
        public async Task TestDeletarFuncionarioInexistente()
        {
            // Arrange
            int funcionarioID = 999; // ID que não existe

            // Act
            var result = await _controller.DeleteFuncionario(funcionarioID);

            // Assert
           // Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
