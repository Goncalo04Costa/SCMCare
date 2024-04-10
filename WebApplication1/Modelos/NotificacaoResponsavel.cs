using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class NotificacaoResponsavel
    {
        public int NotificacaoId { get; set; }

        [Required]
        public int ResponsavelId { get; set; }

        public int Estado { get; set; }
    }
}