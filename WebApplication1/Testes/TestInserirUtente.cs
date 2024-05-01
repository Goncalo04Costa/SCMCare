using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Testes
{
    [TestFixture]
    public class TestInserirUtente
    {
        private UtentesController _controller;
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
            _controller = new UtentesController(dbContext);
        }

        [Test]
        public async Task InserirUtente_DeveRetornarOk_QuandoUtenteForValido()
        {
            // Arrange
            var utente = new Utente
            {
                Nome = "Maria",
                NIF = 123456789,
                SNS = 987654321,
                DataAdmissao = DateTime.Now,
                Historico = false
            };

            // Act
            var result = await _controller.InserirUtente(utente);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsInstanceOf<OkObjectResult>(result.Result); // Verifica se o resultado é um OkObjectResult

            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual("Utente adicionado com sucesso", okResult.Value); // Verifica a mensagem de sucesso
        }

        [Test]
        public async Task InserirUtente_DeveRetornarBadRequest_QuandoUtenteForNulo()
        {
            // Arrange
            Utente utente = null;

            // Act 
            var result = await _controller.InserirUtente(utente);

            // Assert 
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result); // Verifica se o resultado é um BadRequestObjectResult

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.AreEqual("Objeto inválido", badRequestResult.Value); // Verifica a mensagem de erro
        }
    }
}
