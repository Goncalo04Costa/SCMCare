using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Excecoes;
namespace Modelos
{
    public class UserFuncionario
    {
        public int FuncionariosId { get; private set; }

        public string Nome { get;  set; }

        public string Email { get;  set; }

        public byte[] PasseH { get; private set; }

        public byte[] PasseS { get; private set; }


        public UserFuncionario() { }
        public UserFuncionario(int funcionariosId, string nome, string email)
        {
            DomainExceptionValidation.When( funcionariosId < 0, "Erro");
            FuncionariosId = funcionariosId;
            ValidateDomain(nome, email);
        }

        public UserFuncionario(string nome, string email)
        {
            ValidateDomain(nome, email);
        }

        public void AlterarSenha(byte[] passwordHash, byte[] passwordSalt)
        {
            PasseH = passwordHash;
            PasseS = passwordSalt;
        }

        private void ValidateDomain(string nome, string email)
        {
            DomainExceptionValidation.When(nome.Length == 0, "O nome é obrigatorio");
            DomainExceptionValidation.When(email.Length == 0, "O eamail é obigatório");
            DomainExceptionValidation.When(nome.Length > 250, "O nome nao pode ultrapassar de 250 caracteres");
            DomainExceptionValidation.When(email.Length > 200, "O email nao pode ultrapassar de 200 caractere");
            Nome = nome;
            Email = email;
        }
    }
}
