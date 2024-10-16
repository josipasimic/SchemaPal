using FluentResults;
using SchemaPal.DataTransferObjects;

namespace SchemaPal.Services.SchemaMakerServices
{
    public class SchemaInjectionService : ISchemaInjectionService
    {
        private readonly ISchemaPalApiService _schemaPalApiService;
        private readonly IJsonService _jsonService;
        private readonly IResultProcessor _resultProcessor;

        public Guid InjectedSchemaId { get; set; } = Guid.Empty;
        public string InjectedSchemaName { get; set; } = string.Empty;
        public DatabaseSchema InjectedSchema { get; set; } = null;

        public SchemaInjectionService(ISchemaPalApiService schemaPalApiService, 
            IJsonService jsonService,
            IResultProcessor resultProcessor)
        {
            _schemaPalApiService = schemaPalApiService;
            _jsonService = jsonService;
            _resultProcessor = resultProcessor;
        }

        public async Task<Result> PushSchemaFromAPI(Guid schemaId)
        {
            var result = await _schemaPalApiService.GetDatabaseSchema(schemaId);

            if (result.IsSuccess)
            {
                var schemaRecord = result.Value;
                var schema = _jsonService.Deserialize<DatabaseSchema>(schemaRecord.SchemaJsonFormat);

                InjectedSchema = schema;
                InjectedSchemaId = schemaRecord.Id;
                InjectedSchemaName = schemaRecord.Name;

                return Result.Ok();
            }

            var errorMessage = await _resultProcessor.ProcessFailedResult(result);
            return Result.Fail(errorMessage);
        }

        public void PushSchemaFromJsonImport(string jsonSchema)
        {

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
