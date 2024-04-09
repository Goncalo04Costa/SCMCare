using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class NotificacaoFuncionario
    {
        public int NotificacoesId { get; set; }

        [Required]
        public int FuncionariosId { get; set; }

        public int Estado { get; set; }
    }
}