using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Utentes
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int NIF { get; set; }

        [Required]
        public int SNS { get; set; }

        [Required]
        public DateTime DataAdmissao { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public bool Historico { get; set; }

        [Required]
        public bool Tipo { get; set; }

        [Required]
        public int TiposAdmissaoId { get; set; }

        public string MotivoAdmissao { get; set; }
        public string DiagnosticoAdmissao { get; set; }
        public string Observacoes { get; set; }
        public string NotaAdmissao { get; set; }
        public string AntecedentesPessoais { get; set; }
        public string ExameObjetivo { get; set; }

        [Required]
        public double Mensalidade { get; set; }

        [Required]
        public double Cofinanciamento { get; set; }
    }
}