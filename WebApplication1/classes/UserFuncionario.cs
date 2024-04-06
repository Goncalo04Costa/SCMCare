using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserFuncionario
    {
        public int FuncionariosId { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Passe { get; set; }
    }
}