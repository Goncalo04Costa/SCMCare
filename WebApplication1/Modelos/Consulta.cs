using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Consulta
    {
        public int Id { get; set; }

        public DateTime? Data { get; set; }

        public string Descricao { get; set; }

        [Required]
        public int HospitaisId { get; set; }

        [Required]
        public int UtentesId { get; set; }

        public int? FuncionariosId { get; set; }

        public int? ResponsaveisId { get; set; }
    }
}