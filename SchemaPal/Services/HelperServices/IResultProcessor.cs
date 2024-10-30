using FluentResults;

namespace SchemaPal.Services.HelperServices
{
    public interface IResultProcessor
    {
        Task<string> ProcessFailedResult<T>(Result<T> result);
    }
}
