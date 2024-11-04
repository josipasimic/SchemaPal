using FluentAssertions;
using SchemaPal.DataTransferObjects;

namespace SchemaPal.IntegrationTests.Api.SchemaPalApi.Authentication
{
    public class LoginTests : SchemaPalApiTestsBase
    {
        [Fact]
        public async Task GivenUserData_WhenLoggingIn_ThenResultExpected()
        {
            var username = Guid.NewGuid().ToString();
            var password = "Password1122";

            await _schemaPalApiService.RegisterUser(new UserRegistration
            {
                Username = username,
                Password = password,
                PasswordConfirmation = password
            });

            var registrationResult = await _schemaPalApiService.LoginUser(new UserLogin
            {
                Username = username,
                Password = password
            });

            registrationResult.IsSuccess.Should().BeTrue();
            registrationResult.Value.Should().NotBeNull();
            registrationResult.Value.Token.Should().NotBeEmpty();
        }
    }
}
