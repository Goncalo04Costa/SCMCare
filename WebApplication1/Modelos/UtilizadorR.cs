using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class UtilizadorR 
    {
       
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName é obrigatório.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "PassWord é obrigatória.")]
        public string Password { get; set; }
    }
}
