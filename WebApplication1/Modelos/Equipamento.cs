using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Equipamento
    {
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public bool Historico { get; set; }

        [Required]
        public int TiposEquipamentoId { get; set; }

        [Required]
        public int QuartosId { get; set; }
    }
}