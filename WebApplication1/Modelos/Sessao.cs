using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Sessao
    {
        public int SessaoId { get; set; }

        [Required]
        public int UtentesId { get; set; }

        [Required]
        public int FuncionarioID { get; set; }


        [Required]
        public DateTime Dia { get; set; }

        public string Observacoes { get; set; }
    }
}