using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Funcionario
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int TiposFuncionarioId { get; set; }

        [Required]
        public bool Historico { get; set; }

        //public ICollection<ContactoFuncionario>? Contactos { get; set; }
    }
}