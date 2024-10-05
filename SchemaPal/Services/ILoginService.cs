namespace SchemaPal.Services
{
    public interface ILoginService
    {
        Task<bool> Login(string username, string password);
        Task Logout();
    }
}
