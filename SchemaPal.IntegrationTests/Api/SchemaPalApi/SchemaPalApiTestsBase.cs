using FluentAssertions;
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

        protected async Task LoginUser(
            string username,
            string password,
            bool shouldRegister = false)
        {
            if (shouldRegister)
            {
                var registrationResult = await _schemaPalApiService.RegisterUser(new UserRegistration
                {
                    Username = username,
                    Password = password,
                    PasswordConfirmation = password
                });

                registrationResult.IsSuccess.Should().BeTrue();
            }

            var loginResult = await _schemaPalApiService.LoginUser(new UserLogin
            {
                Username = username,
                Password = password
            });

            loginResult.IsSuccess.Should().BeTrue();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
