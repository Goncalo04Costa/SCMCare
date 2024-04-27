using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Modelos
{

        public class AuthenticationRequest
        {
            [Required]
            public string UserName { get; set; }
            [Required]
            public string Password { get; set; }
        }
    
}
