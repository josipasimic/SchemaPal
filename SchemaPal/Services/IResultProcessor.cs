using FluentResults;

namespace SchemaPal.Services
{
    public interface IResultProcessor
    {
        Task<string> ProcessFailedResult<T>(Result<T> result);
    }
}
