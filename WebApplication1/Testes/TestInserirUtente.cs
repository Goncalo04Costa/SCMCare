using Nunit.Framework;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using WebApplication1;
using Microsoft.AspNetCore.Routing;
using System.Data.Entity.Core.Objects;


namespace WebApplication1.Testes
{

    [TestFixture]
    public class UtentesControllerTests
    {
        private UtentesController _controller;
        private Mock<AppDbContext> _mockDbContext;


        [SetUp]
        public void SetUp()
        {
            _mockDbContext = new Mock<AppDbContext>();
            _controller = new UtentesController(_mockDbContext.Object);
        }

        [Test]
        public async Task InserirUtente_WithValidUtente_ReturnsOk()
        {
            // Arrange
            var utente = new Utente
            {
                Id = 1,
                Nome = "Teste",

            };

            // Act
            var result = await _controller.InserirUtente(utente) as OKObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Utente adicionado com sucesso", result.Value);
        }

        [Test]
        public async Task InserirUtente_WithNullUtente_ReturnsBadRequest()
        {
            // Arrange
            Utente utente = null;

            // Act 
            var result = await _controller.InserirUtente(utente) as BadRequestObjectResult;

            // Assert 
            Assert.IsNotNull(result);
            Assert.AreEqual("Objeto inválido", result.Value);
        }
    }
}