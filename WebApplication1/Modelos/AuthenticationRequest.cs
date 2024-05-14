using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Modelos
{
    //classe que representa os dados dos pedidos de entrada

    public class AuthenticationRequest //autenticar um utelizador atraves de um username e uma passwaord
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; }

    }
}
