using FluentResults;
using SchemaPal.DataTransferObjects.API;

namespace SchemaPal.Services
{
    public interface ISchemaPalApiService
    {
        Task<Result> RegisterUser(UserRegistration userRegistration);

        Task<Result<AccessToken>> LoginUser(UserLogin userLogin);

        Task<Result<Guid>> SaveDatabaseSchema(ExtendedSchemaRecord extendedSchemaRecord);

        Task<Result<List<ShortSchemaRecord>>> GetDatabaseSchemasForLoggedInUser();

        Task<Result<ExtendedSchemaRecord>> GetDatabaseSchema(Guid id);

        Task<Result> DeleteDatabaseSchema(Guid id);
    }
}
