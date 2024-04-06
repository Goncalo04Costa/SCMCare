using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Turno
    {
        public int Id { get; set; }

        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        public TimeSpan HoraFim { get; set; }

        [Required]
        public bool Ativo { get; set; }
    }
}