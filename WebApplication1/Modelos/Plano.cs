using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Plano
    {
        public int Id { get; set; }

        [Required]
        public int UtentesId { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public string Observacoes { get; set; }
    }
}