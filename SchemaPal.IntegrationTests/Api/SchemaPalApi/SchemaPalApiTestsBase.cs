using Moq;
using SchemaPal.DataTransferObjects;
using SchemaPal.Services.UserServices;

namespace SchemaPal.IntegrationTests.Api.SchemaPalApi
{
    public class SchemaPalApiTestsBase
    {
        protected HttpClient _httpClient;
        protected readonly ISchemaPalApiService _schemaPalApiService;

        public SchemaPalApiTestsBase()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7034/api/");

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(factory => factory.CreateClient("SchemaPalApi"))
                .Returns(_httpClient);

            _schemaPalApiService = new SchemaPalApiService(mockHttpClientFactory.Object);
        }

        protected async Task<AccessToken> LoginUser(
            string username,
            string password,
            bool shouldRegister = false)
        {
            if (shouldRegister)
            {
                await _schemaPalApiService.RegisterUser(new UserRegistration
                {
                    Username = username,
                    Password = password,
                    PasswordConfirmation = password
                });
            }

            var registrationResult = await _schemaPalApiService.LoginUser(new UserLogin
            {
                Username = username,
                Password = password
            });

            return registrationResult.Value;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
