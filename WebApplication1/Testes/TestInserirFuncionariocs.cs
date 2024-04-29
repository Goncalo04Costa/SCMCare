using NUnit.Framework;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApplication1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace WebApplication1.Testes
{
    [TestFixture]
    public class FuncionariosControllerTests
    {
        private FuncionariosController _controller;
        private DbContextOptions<AppDbContext> _options;

        [SetUp]
        public void Setup()
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

        [Test]
        public async Task InserirFuncionario_DeveRetornarOk_QuandoFuncionarioForValido()
        {
            // Arrange 
            var funcionario = new Funcionario { Nome = "João", TipoFuncionarioId = 1, Historico = false };

            // Act 
            var result = await _controller.InserirFuncionario(funcionario);

            // Assert
            Assert.IsInstanceOf > OKObjectResult > (result.Result);
        }
    }
}
