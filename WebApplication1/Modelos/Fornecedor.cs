using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Fornecedor
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }
    }
}