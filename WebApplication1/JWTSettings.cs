namespace WebApplication1
{
    public class JwtSettings 
    {
        public string secretKey { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }

        public int TokenExpiration { get; set; }
    }
}
