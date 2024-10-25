using Microsoft.JSInterop;
using SchemaPal.Helpers.SchemaMakerHelpers;
using SchemaPal.SchemaObjects;

namespace SchemaPal.Services.SchemaMakerServices
{
    public class ExportService : IExportService
    {
        private const string ExportPngJavaScriptFunctionName = "exportDivToPng";

        private const string ExportJsnJavaScriptFunctionName = "saveAsFile";
        private const string ExportJsnFileName = "schema.json";

        private readonly IJSRuntime _jsRuntime;
        private readonly IJsonConverter _jsonConverter;

        public ExportService(IJSRuntime jsRuntime, IJsonConverter jsonConverter)
        {
            _jsRuntime = jsRuntime;
            _jsonConverter = jsonConverter;
        }

        public async Task ExportSchemaAsPng()
        {
            await _jsRuntime.InvokeVoidAsync(
                ExportPngJavaScriptFunctionName,
                SchemaMakerConstants.ExportPngDivId);
        }

        public async Task ExportSchemaAsJson(DatabaseSchema databaseSchema)
        {
            var databaseSchemaInJsonFormat = _jsonConverter.Serialize(databaseSchema);

            await _jsRuntime.InvokeVoidAsync(
                ExportJsnJavaScriptFunctionName,
                ExportJsnFileName,
                databaseSchemaInJsonFormat);
        }
    }
}
