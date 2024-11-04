using FluentResults;
using SchemaPal.DataTransferObjects;

namespace SchemaPal.Services.UserServices
{
    public interface ISchemaPalApiService
    {
        Task<Result> RegisterUser(UserRegistration userRegistration);

        Task<Result> LoginUser(UserLogin userLogin);

        Task<Result<Guid>> SaveDatabaseSchema(ExtendedSchemaRecord extendedSchemaRecord);

        Task<Result<List<ShortSchemaRecord>>> GetDatabaseSchemasForLoggedInUser();

        Task<Result<ExtendedSchemaRecord>> GetDatabaseSchema(Guid id);

        Task<Result> DeleteDatabaseSchema(Guid id);
    }
}
