using SchemaPal.DataTransferObjects;
namespace SchemaPal.Services
{
    public interface ISchemaPalApiService
    {
        Task<int> ValidateUser(User user);

        Task<int> CreateUser(User user);

        Task<int> SaveDatabaseSchema(DatabaseSchema databaseSchema);

        Task<List<(int Id, string Name)>> GetDatabaseSchemasForUser(int userId);

        Task<DatabaseSchema> GetDatabaseSchema(int id);

        Task<bool> DeleteDatabaseSchema(int id);
    }
}
