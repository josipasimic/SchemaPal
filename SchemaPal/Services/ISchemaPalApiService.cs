using FluentResults;
using SchemaPal.DataTransferObjects;
using SchemaPal.DataTransferObjects.API;

namespace SchemaPal.Services
{
    public interface ISchemaPalApiService
    {
        Task<Result> RegisterUser(UserRegistration userRegistration);

        Task<Result> LoginUser(UserLogin userLogin);

        // -----------------------------------------

        Task<int> SaveDatabaseSchema(DatabaseSchema databaseSchema);

        Task<List<(int Id, string Name)>> GetDatabaseSchemasForUser(int userId);

        Task<DatabaseSchema> GetDatabaseSchema(int id);

        Task<bool> DeleteDatabaseSchema(int id);
    }
}
