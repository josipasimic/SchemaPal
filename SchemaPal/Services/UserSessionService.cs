using Blazored.SessionStorage;
using FluentResults;
using SchemaPal.DataTransferObjects.API;
using SchemaPal.Helpers;

namespace SchemaPal.Services
{
    public class UserSessionService : IUserSessionService
    {
        private const string IsGuestKey = "isGuest";
        private const string IsLoggedInKey = "isLoggedIn";
        private const string UsernameKey = "loggedInUsername";
        private const string AccessTokenKey = "accessToken";

        private readonly ISessionStorageService _sessionStorage;

        public UserSessionService(ISessionStorageService sessionStorageService) 
        { 
            _sessionStorage = sessionStorageService;
        }

        public async Task StartGuestUserSession()
        {
            await _sessionStorage.SetItemAsync(IsLoggedInKey, false);
            await _sessionStorage.RemoveItemAsync(UsernameKey);
            await _sessionStorage.RemoveItemAsync(AccessTokenKey);

            await _sessionStorage.SetItemAsync(IsGuestKey, true);
        }

        public async Task<Result> StartLoggedInUserSession(string username, Result<AccessToken> apiLoginResult)
        {
            if (apiLoginResult.IsFailed)
            {
                var errorMessage = apiLoginResult.Errors.First().Message;
                return Result.Fail(errorMessage);
            }

            await _sessionStorage.SetItemAsync(IsGuestKey, false);

            await _sessionStorage.SetItemAsync(IsLoggedInKey, true);
            await _sessionStorage.SetItemAsync(UsernameKey, username);

            var accessToken = apiLoginResult.Value;
            await _sessionStorage.SetItemAsync(AccessTokenKey, accessToken.Token);

            return Result.Ok();
        }

        public async Task<bool> IsUserGuest()
        {
            var isGuest = await _sessionStorage.GetItemAsync<bool>(IsGuestKey);

            return isGuest;
        }

        public async Task<bool> IsUserLoggedIn()
        {
            var isLoggedIn = await _sessionStorage.GetItemAsync<bool>(IsLoggedInKey);
            var accessToken = await _sessionStorage.GetItemAsync<string>(AccessTokenKey);

            if (isLoggedIn
                && !string.IsNullOrEmpty(accessToken))
            {
                return true;
            }

            return false;
        }

        public async Task<string> GetLoggedInUsername()
        {

           var username = await _sessionStorage.GetItemAsync<string>(UsernameKey);

            return username;
        }

        public async Task EndUserSession()
        {
            await _sessionStorage.SetItemAsync(IsLoggedInKey, false);
            await _sessionStorage.RemoveItemAsync(UsernameKey);
            await _sessionStorage.RemoveItemAsync(AccessTokenKey);

            await UserSectionComponentHelper.NotifyUserStatusChange();
        }
    }
}
