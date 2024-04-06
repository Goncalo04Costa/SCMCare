using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Sobremesa
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public bool Tipo { get; set; }
    }
}
