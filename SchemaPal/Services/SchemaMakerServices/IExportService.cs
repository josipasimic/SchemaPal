using SchemaPal.DataTransferObjects;

namespace SchemaPal.Services.SchemaMakerServices
{
    public interface IExportService
    {
        Task ExportSchemaAsPng();

        Task ExportSchemaAsJson(DatabaseSchema databaseSchema);
    }
}
