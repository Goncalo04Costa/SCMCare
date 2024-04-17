using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Hospital
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Morada { get; set; }

        public bool Ativo { get; set; }
    }
}