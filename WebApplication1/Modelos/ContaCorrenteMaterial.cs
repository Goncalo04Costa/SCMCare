using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class ContaCorrenteMaterial
    {
        public int Id { get; set; }

        public string Fatura { get; set; }

        [Required]
        public int MateriaisId { get; set; }

        public int? PedidosMaterialId { get; set; }

        [Required]
        public int FuncionariosId { get; set; }

        public int? UtentesId { get; set; }

        [Required]
        public System.DateTime Data { get; set; }

        [Required]
        public bool Tipo { get; set; }

        [Required]
        public int QuantidadeMovimento { get; set; }

        public string Observacoes { get; set; }
    }
}