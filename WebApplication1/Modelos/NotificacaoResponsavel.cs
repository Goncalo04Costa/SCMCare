using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class NotificacaoResponsavel
    {
        public int Id { get; set; }

        [Required]
        public string Mensagem { get; set; }

        public int Estado { get; set; }

        [Required]
        public int ResponsaveisId { get; set; }
    }
}