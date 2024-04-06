using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
        public class ContactoFornecedor
    {
        public int FornecedoresId { get; set; }
        public int TipoContactoId { get; set; }

        [Required]
        public string Valor { get; set; }
    }
}