using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserResponsavel
    {
        public int ResponsaveisId { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Passe { get; set; }
    }
}