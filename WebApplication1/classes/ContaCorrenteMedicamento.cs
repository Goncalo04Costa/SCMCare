using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ContaCorrenteMedicamento
    {
        public int Id { get; set; }

        public string Fatura { get; set; }

        [Required]
        public int MedicamentosId { get; set; }

        public int? PedidosMedicamentoId { get; set; }

        [Required]
        public int FuncionariosId { get; set; }

        [Required]
        public int UtentesId { get; set; }

        [Required]
        public System.DateTime Data { get; set; }

        [Required]
        public bool Tipo { get; set; }

        [Required]
        public int QuantidadeMovimento { get; set; }

        public string Observacoes { get; set; }
    }
}