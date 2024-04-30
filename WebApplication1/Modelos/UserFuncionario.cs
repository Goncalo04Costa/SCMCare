using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Modelos
{
    public class UserFuncionario : IdentityUser
    {
        [Required]
        public int FuncionarioId { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Passe { get; set; }
    }
}
