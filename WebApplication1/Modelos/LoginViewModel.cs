using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Modelos
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
