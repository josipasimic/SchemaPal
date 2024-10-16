using Blazored.SessionStorage;
using FluentResults;
using Microsoft.AspNetCore.Components;
using SchemaPal.DataTransferObjects.API;

namespace SchemaPal.Services
{
    public class UserSessionService : IUserSessionService
    {
        private const string IsGuestKey = "isGuest";
        private const string IsLoggedInKey = "isLoggedIn";
        private const string UsernameKey = "loggedInUsername";
        private const string AccessTokenKey = "accessToken";
        private const string AccessTokenExpirationKey = "accessTokenExpirationUtc";

        private readonly ISessionStorageService _sessionStorage;
        private readonly NavigationManager _navigationManager;

        public UserSessionService(ISessionStorageService sessionStorageService,
            NavigationManager navigationManager) 
        { 
            _sessionStorage = sessionStorageService;
            _navigationManager = navigationManager;
        }

        public async Task StartGuestUserSession()
        {
            await _sessionStorage.SetItemAsync(IsLoggedInKey, false);
            await _sessionStorage.RemoveItemAsync(UsernameKey);
            await _sessionStorage.RemoveItemAsync(AccessTokenKey);
            await _sessionStorage.RemoveItemAsync(AccessTokenExpirationKey);

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
            await _sessionStorage.SetItemAsync(AccessTokenExpirationKey, accessToken.ExpirationDateUtc);

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
            var accessTokenExpirationUtc = await _sessionStorage.GetItemAsync<DateTime>(AccessTokenExpirationKey);

            if (isLoggedIn
                && !string.IsNullOrEmpty(accessToken)
                && accessTokenExpirationUtc > DateTime.UtcNow)
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

        public async Task EndUserSession(LogoutReason logoutReason)
        {
            await _sessionStorage.SetItemAsync(IsLoggedInKey, false);
            await _sessionStorage.RemoveItemAsync(UsernameKey);
            await _sessionStorage.RemoveItemAsync(AccessTokenKey);
            await _sessionStorage.RemoveItemAsync(AccessTokenExpirationKey);

            if (logoutReason == LogoutReason.TokenExpired)
            {
                _navigationManager.NavigateTo("/login-redirect?sessionExpired=1");
            }
        }
    }

    public enum LogoutReason
    {
        UserRequest,
        TokenExpired
    }
}
