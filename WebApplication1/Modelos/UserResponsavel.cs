using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class UserResponsavel
    {
        [Key]
        public int ResponsaveisId { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Passe { get; set; }
    }
}
