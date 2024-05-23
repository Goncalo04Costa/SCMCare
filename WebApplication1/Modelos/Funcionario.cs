using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Funcionario
    {
        public int FuncionarioID { get; set; } 

        public int IDFuncionario { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int TiposFuncionarioId { get; set; }

        [Required]
        public bool Historico { get; set; }

    }
}
