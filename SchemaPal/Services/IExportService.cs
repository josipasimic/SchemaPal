using SchemaPal.DataTransferObjects;
using SchemaPal.Pages.SchemaMakerHelpers;

namespace SchemaPal.Services
{
    public interface IExportService
    {
        Task ExportSchemaAsPng();

        Task ExportSchemaAsJson(DatabaseSchema databaseSchema);
    }
}
