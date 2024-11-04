using FluentAssertions;
using FluentResults;
using SchemaPal.DataTransferObjects;
using System.Net;

namespace SchemaPal.IntegrationTests.Api.SchemaPalApi.Authentication
{
    public class RegistrationTests : SchemaPalApiTestsBase
    {
        [Theory]
        [MemberData(nameof(RegistrationTestCases))]
        public async Task GivenUserData_WhenRegistrating_ThenResultExpected(
            UserRegistration userRegistration,
            Result expectedResult)
        {
            var registrationResult = await _schemaPalApiService.RegisterUser(userRegistration);

            registrationResult.Should().BeEquivalentTo(expectedResult);
        }

        public static TheoryData<UserRegistration, Result> RegistrationTestCases()
        {
            return new TheoryData<UserRegistration, Result>
            {
                 {
                    new UserRegistration
                    {
                        Username = Guid.NewGuid().ToString(),
                        Password = "password123",
                        PasswordConfirmation = "password123"
                    },
                    Result.Ok()
                },
                {
                    new UserRegistration
                    {
                        Username = Guid.NewGuid().ToString(),
                        Password = "password123",
                        PasswordConfirmation = "password124"
                    },
                    Result.Fail($"Registracija nije uspjela! HTTP kod greške: {(int)HttpStatusCode.BadRequest}. Poruka: Bad Request")
                },
                {
                    new UserRegistration
                    {
                        Username = string.Empty,
                        Password = "password123",
                        PasswordConfirmation = "password124"
                    },
                    Result.Fail($"Registracija nije uspjela! HTTP kod greške: {(int)HttpStatusCode.BadRequest}. Poruka: Bad Request")
                },
                {
                    new UserRegistration
                    {
                        Username = Guid.NewGuid().ToString(),
                        Password = string.Empty,
                        PasswordConfirmation = string.Empty
                    },
                    Result.Fail($"Registracija nije uspjela! HTTP kod greške: {(int)HttpStatusCode.BadRequest}. Poruka: Bad Request")
                }
            };
        }
    }
}
