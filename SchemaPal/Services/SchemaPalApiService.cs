using Blazored.SessionStorage;
using FluentResults;
using SchemaPal.DataTransferObjects;
using SchemaPal.DataTransferObjects.API;
using System.Net;
using System.Net.Http.Json;

namespace SchemaPal.Services
{
    public class SchemaPalApiService : ISchemaPalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IJsonService _jsonService;
        private readonly ISessionStorageService _sessionStorage;

        public SchemaPalApiService(IHttpClientFactory httpClientFactory,    
            ISessionStorageService sessionStorage,
            IJsonService jsonService)
        {
            _httpClient = httpClientFactory.CreateClient("SchemaPalApi");
            _sessionStorage = sessionStorage;
            _jsonService = jsonService;
        }

        public async Task<Result> RegisterUser(UserRegistration userRegistration)
        {
            var response = await _httpClient.PostAsJsonAsync("Authentication/register", userRegistration);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Result.Fail("Registracija nije uspjela! Podaci za registraciju su neispravni.");
                }

                return Result.Fail($"Registracija nije uspjela! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Registracija nije uspjela! {ex.Message}");
            }

            return Result.Ok();
        }

        public async Task<Result> LoginUser(UserLogin userLogin)
        {
            var response = await _httpClient.PostAsJsonAsync("Authentication/login", userLogin);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized
                    || ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Result.Fail("Prijava nije uspjela! Podaci za prijavu su neispravni.");
                }

                return Result.Fail($"Prijava nije uspjela! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Prijava nije uspjela! {ex.Message}");
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResult>();

            await _sessionStorage.SetItemAsync("authToken", result.Token);

            return Result.Ok();
        }

        public async Task<int> SaveDatabaseSchema(DatabaseSchema databaseSchema)
        {
            var databaseSchemaJson = _jsonService.Serialize(databaseSchema);

            return 0;
        }

        public async Task<List<(int Id, string Name)>> GetDatabaseSchemasForUser(int userId)
        {
            //var listFromApi = new List<ShortSchemaRecord>
            //{
            //    new ShortSchemaRecord
            //    {
            //        Id = 1,
            //        Name = "Schema 1"
            //    },
            //    new ShortSchemaRecord
            //    {
            //        Id = 2,
            //        Name = "Schema 2"
            //    },
            //    new ShortSchemaRecord
            //    {
            //        SchemaId = 3,
            //        SchemaName = "Schema 3"
            //    }
            //};

            //var returnList = listFromApi.Select(x => (x.SchemaId, x.SchemaName)).ToList();

            return new List<(int Id, string Name)>();
        }

        public async Task<DatabaseSchema> GetDatabaseSchema(int id)
        {
            var resultFromApi = new ExtendedSchemaRecord();
            var databaseSchema = _jsonService.Deserialize<DatabaseSchema>(resultFromApi.SchemaJsonFormat);

            return databaseSchema;
        }

        public async Task<bool> DeleteDatabaseSchema(int id)
        {
            return false;
        }
    }
}
