using FluentResults;
using SchemaPal.Helpers.SchemaMakerHelpers;
using SchemaPal.SchemaObjects;
using SchemaPal.Services.HelperServices;
using SchemaPal.Services.UserServices;

namespace SchemaPal.Services.SchemaMakerServices
{
    public class SchemaInjectionService : ISchemaInjectionService
    {
        private readonly ISchemaPalApiService _schemaPalApiService;
        private readonly IJsonConverter _jsonConverter;
        private readonly IResultProcessor _resultProcessor;

        public Guid InjectedSchemaId { get; set; } = Guid.Empty;
        public string InjectedSchemaName { get; set; } = string.Empty;
        public DatabaseSchema InjectedSchema { get; set; } = null;

        public SchemaInjectionService(ISchemaPalApiService schemaPalApiService, 
            IJsonConverter jsonConverter,
            IResultProcessor resultProcessor)
        {
            _schemaPalApiService = schemaPalApiService;
            _jsonConverter = jsonConverter;
            _resultProcessor = resultProcessor;
        }

        public async Task<Result> PushSchemaFromAPI(Guid schemaId)
        {
            var result = await _schemaPalApiService.GetDatabaseSchema(schemaId);

            if (result.IsSuccess)
            {
                var schemaRecord = result.Value;
                var schema = _jsonConverter.Deserialize<DatabaseSchema>(schemaRecord.SchemaJsonFormat);

                InjectedSchema = schema;
                InjectedSchemaId = schemaRecord.Id;
                InjectedSchemaName = schemaRecord.Name;

                return Result.Ok();
            }

            var errorMessage = await _resultProcessor.ProcessFailedResult(result);
            return Result.Fail(errorMessage);
        }

        public Result PushSchemaFromJsonImport(string jsonSchema)
        {
            var databaseSchema = (DatabaseSchema)null;

            try
            {
                databaseSchema = _jsonConverter.Deserialize<DatabaseSchema>(jsonSchema);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Došlo je do pogreške kod obrade sadržaja datoteke: {ex.Message}");
            }

            if (databaseSchema is null)
            {
                return Result.Fail("Došlo je do pogreške kod obrade sadržaja datoteke.");
            }

            InjectedSchema = databaseSchema;
            InjectedSchemaId = Guid.Empty;
            InjectedSchemaName = SchemaMakerConstants.DefaultNewSchemaName;

            return Result.Ok();
        }

        public (Guid Id, string Name, DatabaseSchema Schema) PopSchema()
        {
            var injectedSchemaId = InjectedSchemaId;
            InjectedSchemaId = Guid.Empty;

            var injectedSchemaName = InjectedSchemaName;
            InjectedSchemaName = string.Empty;

            var injectedSchema = InjectedSchema;
            InjectedSchema = null;

            return (injectedSchemaId, injectedSchemaName, injectedSchema);
        }
    }
}
