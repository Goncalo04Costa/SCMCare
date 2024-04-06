using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class TipoAlergia
    {
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }
    }
}