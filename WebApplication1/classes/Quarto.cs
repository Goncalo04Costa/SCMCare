using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Quarto
    {
        public int Id { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public int TiposQuartoId { get; set; }
    }
}