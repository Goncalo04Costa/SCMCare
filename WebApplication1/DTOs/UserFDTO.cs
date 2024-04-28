using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.DTOs
{
    public class UserFDTO
    {
       public int ID { get; set; }

        [Required(ErrorMessage = " é obrigatória.")]
        [MaxLength(250, ErrorMessage = "O email deve ter, no maximo, 250 caracteres.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "A password é obrigatória.")]
        [MaxLength(100, ErrorMessage = "A password deve ter, no maximo, 100 caracteres.")]
        [MinLength(8, ErrorMessage = "A password deve ter, no minimo, 8 caracteres.")]
        [NotMapped]
        public string PassWord { get; set; }
    }
}
