using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class NotificacaoResponsavel
    {
        public int ResponsaveisId { get; set; }

        public int NotificacoesId { get; set; }

        public int Estado { get; set; }
    }
}