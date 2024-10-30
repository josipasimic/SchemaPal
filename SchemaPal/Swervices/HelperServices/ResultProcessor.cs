using FluentResults;
using SchemaPal.Services.UserServices;
using System.Net;

namespace SchemaPal.Services.HelperServices
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
            if (result.IsSuccess)
            {
                throw new ArgumentException("Result is not failed.");
            }

            var resultError = result.Errors.First();

            var httpStatusCode = resultError.Metadata.GetValueOrDefault(nameof(HttpStatusCode));
            var unboxedHttpStatusCode = httpStatusCode != null
                ? (HttpStatusCode)httpStatusCode
                : default;

            if (unboxedHttpStatusCode == HttpStatusCode.Unauthorized)
            {
                await _userSessionService.EndUserSession();
                return "Vaša prijava je istekla. Prijavite se ponovno u sustav.";
            }

            var errorMessage = $"Došlo je do greške: {resultError.Message}.";
            if (unboxedHttpStatusCode != default)
            {
                errorMessage += $" HTTP kod greške: {(int)unboxedHttpStatusCode}.";
            }

            return errorMessage;
        }
    }
}
