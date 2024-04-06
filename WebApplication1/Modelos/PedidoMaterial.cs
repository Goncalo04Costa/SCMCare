using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class PedidoMaterial
    {
        public int Id { get; set; }

        [Required]
        public int MateriaisId { get; set; }

        [Required]
        public int FuncionariosId { get; set; }

        [Required]
        public int QuantidadeTotal { get; set; }

        [Required]
        public DateTime DataPedido { get; set; }

        [Required]
        public int Estado { get; set; }

        public DateTime? DataConclusao { get; set; }
    }

}