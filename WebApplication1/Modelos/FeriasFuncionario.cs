using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class FeriasFuncionario
    {
        public int Id { get; set; }

        [Required]
        public int FuncionariosId { get; set; }

        [Required]
        public int FuncionariosIdValida { get; set; }

        [Required]
        public DateTime Dia { get; set; }

        [Required]
        public int Estado { get; set; }
    }
}