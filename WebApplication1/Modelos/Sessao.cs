using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Sessao
    {
        public int Id { get; set; }

        [Required]
        public int TiposSessaoId { get; set; }

        [Required]
        public int UtentesId { get; set; }

        [Required]
        public int FuncionariosId { get; set; }


        [Required]
        public DateTime Dia { get; set; }

        public string? Observacoes { get; set; }
    }
}