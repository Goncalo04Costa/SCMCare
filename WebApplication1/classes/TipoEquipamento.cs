using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class TipoEquipamento
    {
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }
    }
}