namespace WebApplication1.Modelos
{
    /// <summary>
    /// classe para representar os dados das respostas de saida
    /// </summary>

    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

    }
}
