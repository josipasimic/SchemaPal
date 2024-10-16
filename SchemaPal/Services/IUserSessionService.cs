using FluentResults;
using Microsoft.AspNetCore.Components;
using SchemaPal.DataTransferObjects.API;

namespace SchemaPal.Services
{
    public interface IUserSessionService
    {
        Task StartGuestUserSession();

        Task<Result> StartLoggedInUserSession(string username, Result<AccessToken> apiLoginResult);

        Task<bool> IsUserGuest();

        Task<bool> IsUserLoggedIn();

        Task<string> GetLoggedInUsername();

        Task EndUserSession(LogoutReason logoutReason);
    }
}
