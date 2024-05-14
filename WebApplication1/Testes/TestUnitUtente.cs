﻿using Xunit;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Testes
{
    public class TestUnitUtente
    {
        private UtentesController _controller;
        private DbContextOptions<AppDbContext> _options;

        public TestUnitUtente()
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

        // Método para testar a inserção de um utente válido no sistema.
        [Fact]
        public async Task TestInserirUtenteValido()
        {
            // Arrange
            var utente = new Utente
            {
                Nome = "Maria",
                NIF = 123456789,
                SNS = 987654321,
                DataAdmissao = DateTime.Now,
                Historico = false,
                Tipo = false,
                TiposAdmissaoId = 1,
                AntecedentesPessoais = "antecedentes",
                DiagnosticoAdmissao = "diagnóstico",
                ExameObjetivo = "exame",
                MotivoAdmissao = "motivo",
                NotaAdmissao = "nota",
                Observacoes = "observações",
                Mensalidade = 100.00,
                Cofinanciamento = 50.00
            };

            // Act
            var result = await _controller.InserirUtente(utente);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<OkObjectResult>(result.Result); // Verifica se o resultado é do tipo OkObjectResult

            var okResult = result.Result as OkObjectResult;
            Assert.Equal("Utente adicionado com sucesso", okResult.Value); // Verifica a mensagem de sucesso
        }

        // Método para testar a tentativa de inserir um utente nulo no sistema.
        [Fact]
        public async Task TestInserirUtenteNulo()
        {
            // Arrange
            Utente utente = null;

            // Act 
            var result = await _controller.InserirUtente(utente);

            // Assert 
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<BadRequestObjectResult>(result.Result); // Verifica se o resultado é do tipo BadRequestObjectResult

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("Objeto inválido", badRequestResult.Value); // Verifica a mensagem de erro
        }

        // Método para testar se não é possível inserir um utente com um NIF que já existe.
        [Fact]
        public async Task TestInserirUtenteComNIFDuplicado()
        {
            // Arrange
            var utente1 = new Utente
            {
                Nome = "Maria",
                NIF = 123456789,
                SNS = 987654321,
                DataAdmissao = DateTime.Now,
                Historico = false,
                Tipo = false,
                TiposAdmissaoId = 1,
                AntecedentesPessoais = "antecedentes",
                DiagnosticoAdmissao = "diagnóstico",
                ExameObjetivo = "exame",
                MotivoAdmissao = "motivo",
                NotaAdmissao = "nota",
                Observacoes = "observações",
                Mensalidade = 100.00,
                Cofinanciamento = 50.00
            };

            // Inserindo o primeiro utente
            await _controller.InserirUtente(utente1);

            // Criando um segundo utente com o mesmo NIF
            var utente2 = new Utente
            {
                Nome = "João",
                NIF = 123456789, // NIF duplicado
                SNS = 123456789,
                DataAdmissao = DateTime.Now,
                Historico = false,
                Tipo = false,
                TiposAdmissaoId = 1,
                AntecedentesPessoais = "antecedentes",
                DiagnosticoAdmissao = "diagnóstico",
                ExameObjetivo = "exame",
                MotivoAdmissao = "motivo",
                NotaAdmissao = "nota",
                Observacoes = "observações",
                Mensalidade = 100.00,
                Cofinanciamento = 50.00
            };

            // Act
            var result = await _controller.InserirUtente(utente2);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<BadRequestObjectResult>(result.Result); // Verifica se o resultado é do tipo BadRequestObjectResult

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("NIF já existe.", badRequestResult.Value); // Verifica a mensagem de erro
        }

        // Método para testar que não seja possível atualizar um utente que não exista na base de dados.
        [Fact]
        public async Task TestAtualizarUtenteInexistente()
        {
            // Arrange
            var idInexistente = 9999; // ID que não existe na base de dados
            var novoUtente = new Utente
            {
                Id = idInexistente,
                Nome = "Novo Nome",
                NIF = 123456789,
                SNS = 987654321,
                DataAdmissao = DateTime.Now,
                Historico = false
            };

            // Act
            var result = await _controller.AtualizaUtente(idInexistente, novoUtente);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result); // Verifica se o resultado é do tipo NotFoundObjectResult

            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal($"Não foi possível encontrar o utente com o ID {idInexistente}", notFoundResult.Value); // Verifica a mensagem de erro
        }

        // Método para testar que não seja possível remover um utente que não exista
        [Fact]
        public async Task TestRemoverUtenteInexistente()
        {
            // Arrange
            int idInexistente = 999; // ID de um utente que não existe na base de dados

            // Act
            var result = await _controller.RemoveUtente(idInexistente);

            

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<NotFoundObjectResult>(result); // Verifica se o resultado é do tipo NotFoundObjectResult

            var notFoundResult = result as NotFoundObjectResult; // Converte o resultado para NotFoundObjectResult
            Assert.NotNull(notFoundResult); // Verifica se a conversão foi bem-sucedida

            // Verifica se a mensagem de erro é a esperada
            Assert.Equal($"Não foi possível encontrar o utente com o ID {idInexistente}", notFoundResult.Value);
            // Ou se preferir, você pode verificar o código de status HTTP
            Assert.Equal(404, notFoundResult.StatusCode); // Verifica se o código de status é 404 (Not Found)

        }

    }
}