using SchemaPal.DataTransferObjects;

namespace SchemaPal.Services
{
    public interface IExportService
    {
        Task ExportSchemaAsPng();

        Task ExportSchemaAsJson(DatabaseSchema databaseSchema);
    }
}
