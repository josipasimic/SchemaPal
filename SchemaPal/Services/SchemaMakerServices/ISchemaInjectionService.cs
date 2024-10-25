using FluentResults;
using SchemaPal.SchemaObjects;

namespace SchemaPal.Services.SchemaMakerServices
{
    public interface ISchemaInjectionService
    {
        Task<Result> PushSchemaFromAPI(Guid schemaId);

        Result PushSchemaFromJsonImport(string jsonSchema);

        (Guid Id, string Name, DatabaseSchema Schema) PopSchema();
    }
}
