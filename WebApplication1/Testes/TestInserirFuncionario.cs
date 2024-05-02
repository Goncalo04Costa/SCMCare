using Xunit;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [Fact]
        public async Task InserirFuncionario_DeveRetornarOk_QuandoFuncionarioForValido()
        {
            // Arrange 
            var funcionario = new Funcionario { Nome = "João", Historico = false };

            // Act 
            var result = await _controller.PostFuncionario(funcionario);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<OkObjectResult>(result); // Verifica se o resultado é do tipo OkObjectResult
        }
    }
}
