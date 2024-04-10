using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class NotificacaoFuncionario
    {
        public int NotificacaoId { get; set; }

        [Required]
        public int FuncionarioId { get; set; }

        public int Estado { get; set; }
    }
}