using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Limpeza
    {
        public int Id { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public int QuartosId { get; set; }

        [Required]
        public int FuncionariosId { get; set; }
    }
}