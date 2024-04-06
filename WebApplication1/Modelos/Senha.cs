using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Senha
    {
        public int FuncionariosId { get; set; }
        public int MenuId { get; set; }

        [Required]
        public int Estado { get; set; }
    }
}