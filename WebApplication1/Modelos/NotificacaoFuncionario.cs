using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class NotificacaoFuncionario
    {
        public int FuncionariosId { get; set; }

        public int NotificacoesId { get; set; }

        public int Estado { get; set; }
    }
}