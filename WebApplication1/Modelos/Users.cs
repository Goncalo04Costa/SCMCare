using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Modelos
{
    public class Users : IdentityUser
    {
        [Required]
        public int ID { get; set; }

        public int IDFuncionario { get; set; }

        public int IDResponsavel { get; set; }

        [Required]
        public string UserName {  get; set; }

        [Required]
        public string Passe { get; set; } // Password property
    }
}
