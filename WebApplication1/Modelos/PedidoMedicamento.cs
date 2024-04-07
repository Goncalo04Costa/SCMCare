using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class PedidoMedicamento
    {
        public int Id { get; set; }

        [Required]
        public int MedicamentosId { get; set; }

        [Required]
        public int FuncionariosId { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public DateTime DataPedido { get; set; }

        [Required]
        public int Estado { get; set; }

        public DateTime? DataConclusao { get; set; }
    }
}