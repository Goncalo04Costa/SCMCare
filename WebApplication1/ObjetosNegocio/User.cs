using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ObjetosNegocio
{
    public class User
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
