using Xunit;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Testes
{
    public class TestInserirUtente
    {
        private UtentesController _controller;
        private DbContextOptions<AppDbContext> _options;

        public TestInserirUtente()
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
                Historico = false
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
                Historico = false
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
                Historico = false
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

            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal($"Não foi possível encontrar o utente com o ID {idInexistente}", notFoundResult.Value); // Verifica a mensagem de erro
        }

        // Método para testar se não é possível atualizar um utente com dados inválidos.
        [Fact]
        public async Task TestAtualizarUtenteComDadosInvalidos()
        {
            // Arrange
            var utenteExistente = new Utente
            {
                Nome = "Maria",
                NIF = 123456789,
                SNS = 987654321,
                DataAdmissao = DateTime.Now,
                Historico = false
            };

            // Adiciona um utente existente
            await _controller.InserirUtente(utenteExistente);

            // Obtem o ID do utente existente
            var idUtenteExistente = (await _controller.ObterUtentePorNome("Maria")).Value.Id;

            // Cria um novo utente inválido
            var utenteInvalido = new Utente
            {
                // Aqui, propositadamente foi deixado o campo "Nome" como nulo, o que é inválido
                NIF = 987654321,
                SNS = 123456789,
                DataAdmissao = DateTime.Now,
                Historico = false
            };

            // Act
            var result = await _controller.AtualizaUtente(idUtenteExistente, utenteInvalido);

            // Assert
            Assert.NotNull(result); // Verifica se o resultado não é nulo
            Assert.IsType<BadRequestObjectResult>(result); // Verifica se o resultado é do tipo BadRequestObjectResult

            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Dados inválidos", badRequestResult.Value); // Verifica a mensagem de erro
        }


    }
}
