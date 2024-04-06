using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class MedicamentoPrescricao
    {
        public int PrescricoesId { get; set; }
        public int MedicamentosId { get; set; }

        [Required]
        public int QuantidadePIntervalo { get; set; }

        [Required]
        public int IntervaloHoras { get; set; }

        [Required]
        public string Instrucoes { get; set; }
    }
}