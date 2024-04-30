namespace WebApplication1
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public int TokenExpiration { get; set; }
    }
}
