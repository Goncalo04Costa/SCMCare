using NUnit.Framework;
using Moq;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using WebApplication1;
using Microsoft.AspNetCore.Routing;
using System.Data.Entity.Core.Objects;


namespace WebApplication1.Testes
{
    [TestFixture]
    public class PedidosMedicamentoControllerTests
    {
        private PedidosMedicamentoController _controller;
        private Mock>AppDbContext> _mockDbContext;

    [SetUp]
        public void SetUp()
        {
            _mockDbContext = new Mock<AppDbContext();
            _controller = new PedidosMedicamentoController(_mockDbContext.Object);
        }

        [Test]
        public async Task InserirPedidoMedicamento_WithValidPedidoMedicamento_ReturnsOk()
        {

            // Arrange
            var pedidoMedicamento = new PedidoMedicamento { /* Defina os atributos do pedido de medicamento aqui */};

            // Act 
            var result = await _controller.InserirPedidoMedicamento(pedidoMedicamento) as OKObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("pedidoMedicamento adicionado com sucesso", result: Value);
        }

        [Test]
        public async Task InserirPedidoMedicamento_WithNullPedidoMedicamento_ReturnsBadRequest()
        {

            // Arrange
            PedidoMedicamento pedidoMedicamento = null;

            // Act 
            var result = await _controller.InserirPedidoMedicamento(pedidoMedicamento) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Objeto inválido", result.Value);
        }
    }
}

