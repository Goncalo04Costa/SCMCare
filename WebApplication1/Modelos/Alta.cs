using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Alta
    {
        public int UtentesId { get; set; }
        public int FuncionariosId { get; set; }

        [Required]
        public DateTime Data { get; set; }

        public string Motivo { get; set; }
        public string Destino { get; set; }
    }
}