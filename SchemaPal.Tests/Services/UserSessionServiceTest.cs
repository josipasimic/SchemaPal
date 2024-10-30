using Blazored.SessionStorage;
using FluentResults;
using SchemaPal.DataTransferObjects;
using SchemaPal.Services.UserServices;
using Moq;
using FluentAssertions;

namespace Tests.Services 
{
    public class UserSessionServiceTest
    {
        [Theory]
        [MemberData(nameof(FailedApiLogIn))]
        [MemberData(nameof(AllowedApiLogiIn))]
        public async Task GivenUsername_WhenStartingLoggedInSession_ThenValidateLogInCorrectly(
            Result<AccessToken> apiLoginResult, 
            Result expectedResult)
        {
            var sessionStorage = new Mock<ISessionStorageService>();
            sessionStorage.Setup(x => x.SetItemAsync(
                It.IsAny<string>(),
                It.IsAny<string>(), 
                It.IsAny<CancellationToken>()));
            sessionStorage.Setup(x => x.SetItemAsync(
                It.IsAny<string>(),
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()));

            var userSessionService = new UserSessionService(sessionStorage.Object);
            var result = await userSessionService.StartLoggedInUserSession("username", apiLoginResult);
            result.Should().BeEquivalentTo(expectedResult);
        }

        public static TheoryData<Result<AccessToken>, Result> AllowedApiLogiIn()
        {
            var accessToken = new AccessToken
            {
                Token = "token"
            };
            var apiLoginResult = accessToken.ToResult().WithSuccess("Success");
            return new TheoryData<Result<AccessToken>, Result>
            {
                { apiLoginResult, Result.Ok()}
            };
        }

        public static TheoryData<Result<AccessToken>, Result> FailedApiLogIn()
        {
            var errorMessage = "Failed log in.";
            var apiLoginResult = new Result<AccessToken>().WithError(errorMessage);
            return new TheoryData<Result<AccessToken>, Result>
            {
                { apiLoginResult, Result.Fail(errorMessage)}
            };
        }

        [Theory]
        [InlineData(true, "token", true)]
        [InlineData(false, "token", false)]
        [InlineData(true, null, false)]
        [InlineData(true, "", false)]
        [InlineData(false, null, false)]
        public async Task GivenSession_WhenCheckingIfUserIsLoggedIn_ThenCorrectlyValidate(
            bool isLoggedIn, 
            string accessToken,
            bool expectedResult)
        {
            var sessionStorage = new Mock<ISessionStorageService>();
            sessionStorage.Setup(x => x.GetItemAsync<bool>(
                "isLoggedIn",
                It.IsAny<CancellationToken>()))
                .Returns(ValueTask.FromResult(isLoggedIn));
            sessionStorage.Setup(x => x.GetItemAsync<string>(
                "accessToken",
                It.IsAny<CancellationToken>()))
                .Returns(ValueTask.FromResult(accessToken));

            var userSessionService = new UserSessionService(sessionStorage.Object);
            var result = await userSessionService.IsUserLoggedIn();
            result.Should().Be(expectedResult);
        }
    }
}