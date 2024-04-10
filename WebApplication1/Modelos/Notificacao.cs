using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Notificacao
    {
        public int Id { get; set; }

        [Required]
        public string Mensagem { get; set; }

        [Required]
        public DateTime Data { get; set; }
    }
}