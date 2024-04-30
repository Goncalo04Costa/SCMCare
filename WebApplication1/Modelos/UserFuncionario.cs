using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Excecoes;
using Microsoft.AspNetCore.Identity;

namespace Modelos
{
    public class UserFuncionario : IdentityUser<string>
    {
        [Key]
        public int FuncionariosId { get; private set; }

        public string Nome { get; set; }

        public override string UserName { get; set; }

        public override string NormalizedUserName { get; set; }

        public override string Email { get; set; }

        public override string NormalizedEmail { get; set; }

        public override bool EmailConfirmed { get; set; }

        public override string PasswordHash { get; set; }

        public override string PhoneNumber { get; set; }

        public override bool PhoneNumberConfirmed { get; set; }

        public override string SecurityStamp { get; set; }

        public override string ConcurrencyStamp { get; set; }
        public override bool TwoFactorEnabled { get; set; }

        public override DateTimeOffset? LockoutEnd { get; set; }

        public override bool LockoutEnabled { get; set; }

        public override int AccessFailedCount { get; set; }

        public UserFuncionario() { }

        public UserFuncionario(int funcionariosId, string nome, string email)
        {
            DomainExceptionValidation.When(funcionariosId < 0, "Erro");
            FuncionariosId = funcionariosId;
            ValidateDomain(nome, email);
        }

        public UserFuncionario(string nome, string email)
        {
            ValidateDomain(nome, email);
            Id = Guid.NewGuid().ToString(); // Generate a unique ID
        }

        public void AlterarSenha(byte[] passwordHash, byte[] passwordSalt)
        {
            PasswordHash = Convert.ToBase64String(passwordHash);
            SecurityStamp = Guid.NewGuid().ToString();
        }

        private void ValidateDomain(string nome, string email)
        {
            DomainExceptionValidation.When(nome.Length == 0, "O nome é obrigatório");
            DomainExceptionValidation.When(email.Length == 0, "O email é obrigatório");
            DomainExceptionValidation.When(nome.Length > 250, "O nome não pode ultrapassar de 250 caracteres");
            DomainExceptionValidation.When(email.Length > 200, "O email não pode ultrapassar de 200 caracteres");
            UserName = nome;
            NormalizedUserName = nome.ToUpper();
            Email = email;
            NormalizedEmail = email.ToUpper();
        }
    }
}
