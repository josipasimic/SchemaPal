using Blazored.SessionStorage;
using System.Net.Http.Json;

namespace SchemaPal.Services
{
    public class LoginService : ILoginService
    {
        private readonly ISessionStorageService _sessionStorage; // Use session storage
        private readonly HttpClient _httpClient;

        public LoginService(ISessionStorageService sessionStorage, HttpClient httpClient)
        {
            _sessionStorage = sessionStorage;
            _httpClient = httpClient;
        }

        public async Task<bool> Login(string username, string password)
        {
            // Call your authentication API
            var response = await _httpClient.PostAsJsonAsync("api/login", new { username, password });

            if (response.IsSuccessStatusCode)
            {
                // Successful login, set isLoggedIn in sessionStorage
                await _sessionStorage.SetItemAsync("isLoggedIn", true);
                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            await _sessionStorage.RemoveItemAsync("isLoggedIn");
        }
    }

}
