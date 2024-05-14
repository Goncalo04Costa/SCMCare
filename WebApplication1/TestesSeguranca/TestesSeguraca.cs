﻿using Xunit;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

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

            }

            var dbContext = new AppDbContext(_options);
            _controller = new PedidosMedicamentoController(dbContext);
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
            // Verifique se as operações são autorizadas apenas para usuários autenticados
            Assert.NotNull(inserirOkResult);
            Assert.NotNull(obterOkResult);
            Assert.NotNull(atualizarOkResult);
            Assert.NotNull(removerOkResult);

            // Verifique se o status de resposta é 401 (Não autorizado) para operações não autorizadas
            Assert.Equal(401, (obterResultado.Result as UnauthorizedResult).StatusCode);
            Assert.Equal(401, (atualizarResultado as UnauthorizedResult).StatusCode);
            Assert.Equal(401, (removerResultado as UnauthorizedResult).StatusCode);
        }
    }
}