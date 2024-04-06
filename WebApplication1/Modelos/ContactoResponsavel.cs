using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class ContactoResponsavel
    {
        public int ResponsaveisId { get; set; }
        public int TipoContactoId { get; set; }

        [Required]
        public string Valor { get; set; }
    }
}