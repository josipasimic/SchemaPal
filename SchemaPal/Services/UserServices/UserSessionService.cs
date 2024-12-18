﻿using Blazored.SessionStorage;
using FluentResults;
using SchemaPal.DataTransferObjects;
using SchemaPal.Services.HelperServices;

namespace SchemaPal.Services.UserServices
{
    public class UserSessionService : IUserSessionService
    {
        private const string IsGuestKey = "isGuest";
        private const string IsLoggedInKey = "isLoggedIn";
        private const string UsernameKey = "loggedInUsername";

        private readonly ISessionStorageService _sessionStorage;
        private readonly IComponentActionsStorage _componentActionsStorage;

        public UserSessionService(ISessionStorageService sessionStorageService,
            IComponentActionsStorage componentActionsStorage)
        {
            _sessionStorage = sessionStorageService;
            _componentActionsStorage = componentActionsStorage;
        }

        public async Task StartGuestUserSession()
        {
            await _sessionStorage.SetItemAsync(IsLoggedInKey, false);
            await _sessionStorage.RemoveItemAsync(UsernameKey);

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
            if (isLoggedIn)
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

            await _componentActionsStorage.InvokeUserSessionTypeChange();
        }
    }
}
