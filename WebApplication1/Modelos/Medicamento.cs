using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Medicamento
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public int Limite { get; set; }

        [Required]
        public int QuantidadeAtual { get; set; }
    }
}