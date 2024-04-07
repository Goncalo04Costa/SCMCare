using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Responsavel
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int UtentesId { get; set; }

        public string Morada { get; set; }
    }
}