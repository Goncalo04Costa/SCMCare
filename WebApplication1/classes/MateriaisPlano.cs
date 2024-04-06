using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class MaterialPlano
    {
        public int PlanosId { get; set; }
        public int MateriaisId { get; set; }

        [Required]
        public int QuantidadePIntervalo { get; set; }

        [Required]
        public int IntervaloHoras { get; set; }

        [Required]
        public string Instrucoes { get; set; }
    }
}