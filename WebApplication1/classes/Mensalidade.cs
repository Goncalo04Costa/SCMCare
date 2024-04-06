using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Mensalidade
    {
        public DateTime Mes { get; set; }
        public DateTime? DataPagamento { get; set; }
        public int UtentesId { get; set; }
        public int? TiposPagamentoId { get; set; }

        [Required]
        public int Estado { get; set; }

    }
}