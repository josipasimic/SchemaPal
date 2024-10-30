using FluentResults;
using SchemaPal.DataTransferObjects;

namespace SchemaPal.Services.UserServices
{
    public interface IUserSessionService
    {
        Task StartGuestUserSession();

        Task<Result> StartLoggedInUserSession(string username, Result<AccessToken> apiLoginResult);

        Task<bool> IsUserGuest();

        Task<bool> IsUserLoggedIn();

        Task<string> GetLoggedInUsername();

        Task EndUserSession();
    }
}
