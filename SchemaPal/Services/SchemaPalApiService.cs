using SchemaPal.DataTransferObjects;
using SchemaPal.DataTransferObjects.API;

namespace SchemaPal.Services
{
    public class SchemaPalApiService : ISchemaPalApiService
    {
        //private const string ApiUrl = "https://api.schemapal.com";

        //private readonly IHttpClient _httpClient;

        private readonly IJsonService _jsonService;

        public SchemaPalApiService(IJsonService jsonService) 
        {
            _jsonService = jsonService;
        }

        public async Task<int> ValidateUser(User user)
        {
            //var client = new HttpClient(ApiUrl);
            //var request = new HttpRequest("users/validate", Method.POST);
            //request.AddJsonBody(user);
            //var response = client.Execute(request);
            //if (response.StatusCode != HttpStatusCode.OK)
            //{
            //    throw new Exception("Invalid user");
            //}

            if (user.Username == "admin" && user.Password == "admin")
            {
               return 1;
            }

            return 0;
        }

        public async Task<int> CreateUser(User user)
        {
            //var client = new HttpClient(ApiUrl);
            //var request = new HttpRequest("users/create", Method.POST);
            //request.AddJsonBody(user);
            //var response = client.Execute(request);
            //if (response.StatusCode != HttpStatusCode.OK)
            //{
            //    throw new Exception("Invalid user");
            //}

            return 0;
        }

        public async Task<int> SaveDatabaseSchema(DatabaseSchema databaseSchema)
        {
            var databaseSchemaJson = _jsonService.Serialize(databaseSchema);

            return 0;
        }

        public async Task<List<(int Id, string Name)>> GetDatabaseSchemasForUser(int userId)
        {
            var listFromApi = new List<ShortSchemaRecordDto>
            {
                new ShortSchemaRecordDto
                {
                    SchemaId = 1,
                    SchemaName = "Schema 1"
                },
                new ShortSchemaRecordDto
                {
                    SchemaId = 2,
                    SchemaName = "Schema 2"
                },
                new ShortSchemaRecordDto
                {
                    SchemaId = 3,
                    SchemaName = "Schema 3"
                }
            };

            var returnList = listFromApi.Select(x => (x.SchemaId, x.SchemaName)).ToList();

            return returnList;
        }

        public async Task<DatabaseSchema> GetDatabaseSchema(int id)
        {
            var resultFromApi = new ExtendedSchemaRecordDto();
            var databaseSchema = _jsonService.Deserialize<DatabaseSchema>(resultFromApi.SchemaInJsonFormat);

            return databaseSchema;
        }

        public async Task<bool> DeleteDatabaseSchema(int id)
        {
            return false;
        }
    }
}
