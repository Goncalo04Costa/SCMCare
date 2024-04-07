using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Consulta
    {
        public int Id { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public int HospitaisId { get; set; }

        [Required]
        public int UtentesId { get; set; }

        [Required]
        public int FuncionariosId { get; set; }

        [Required]
        public int ResponsaveisId { get; set; }
    }
}