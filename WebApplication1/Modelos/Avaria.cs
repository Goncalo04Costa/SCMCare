using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Avaria
    {
        public int Id { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public int EquipamentosId { get; set; }

        public string Descricao { get; set; }

        [Required]
        public int Estado { get; set; }
    }
}