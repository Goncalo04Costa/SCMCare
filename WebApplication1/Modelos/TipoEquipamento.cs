using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class TipoEquipamento
    {
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }
    }
}