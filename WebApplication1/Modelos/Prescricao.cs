using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Prescricao
    {
        public int Id { get; set; }

        [Required]
        public int UtentesId { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        [Required]
        public string Observacoes { get; set; }
    }
}