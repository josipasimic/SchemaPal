using FluentResults;
using System.Net;

namespace SchemaPal.Services
{
    public class ResultProcessor : IResultProcessor
    {
        private readonly IUserSessionService _userSessionService;

        public ResultProcessor(IUserSessionService userSessionService)
        {
            _userSessionService = userSessionService;
        }

        public async Task<string> ProcessFailedResult<T>(Result<T> result)
        {
            var isUserLoginValid = await _userSessionService.IsUserLoggedIn();
            if (!isUserLoginValid)
            {
                await _userSessionService.EndUserSession(LogoutReason.TokenExpired);

                return string.Empty;
            }

            var resultErrorMessage = result.Errors.First().Message;
            var errorMessage = $"Došlo je do greške: {resultErrorMessage}";

            var statusCodeBoxed = result.Errors.First().Metadata.GetValueOrDefault(nameof(HttpStatusCode));
            if (statusCodeBoxed != null)
            {
                var statusCode = (HttpStatusCode)statusCodeBoxed;
                errorMessage += $" HTTP kod greške: {(int)statusCode} {statusCode}";
            }

            return errorMessage;
        }
    }
}
