namespace WebApplication1.Account
{
    public interface IAuthenticate
    {
        Task<bool> AuthenticteAsync(string email, string password);
        Task<bool> UserExists(string email);
        public string GenerateToken(int id, string email);

    }
}
