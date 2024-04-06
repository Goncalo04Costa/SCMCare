using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ContactoFuncionario
    {
        public int FuncionariosId { get; set; }
        public int TipoContactoId { get; set; }

        [Required]
        public string Valor { get; set; }
    }
}