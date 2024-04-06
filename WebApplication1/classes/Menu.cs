using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Menu
    {
        public int Id { get; set; }

        [Required]
        public DateTime Dia { get; set; }

        [Required]
        public bool Horario { get; set; }

        [Required]
        public bool Tipo { get; set; }

        [Required]
        public int SopasId { get; set; }

        [Required]
        public int PratosId { get; set; }

        [Required]
        public int SobremesasId { get; set; }
    }
}