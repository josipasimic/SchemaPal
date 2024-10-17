using Microsoft.JSInterop;
using SchemaPal.DataTransferObjects;
using SchemaPal.Helpers.SchemaMakerHelpers;

namespace SchemaPal.Services.SchemaMakerServices
{
    public class ExportService : IExportService
    {
        private const string ExportPngJavaScriptFunctionName = "exportDivToPng";

        private const string ExportJsnJavaScriptFunctionName = "saveAsFile";
        private const string ExportJsnFileName = "schema.json";

        private readonly IJSRuntime _jsRuntime;
        private readonly IJsonConverter _jsonService;

        public ExportService(IJSRuntime jsRuntime, IJsonConverter jsonService)
        {
            _jsRuntime = jsRuntime;
            _jsonService = jsonService;
        }

        public async Task ExportSchemaAsPng()
        {
            await _jsRuntime.InvokeVoidAsync(
                ExportPngJavaScriptFunctionName,
                SchemaMakerConstants.ExportPngDivId);
        }

        public async Task ExportSchemaAsJson(DatabaseSchema databaseSchema)
        {
            var databaseSchemaInJsonFormat = _jsonService.Serialize(databaseSchema);

            await _jsRuntime.InvokeVoidAsync(
                ExportJsnJavaScriptFunctionName,
                ExportJsnFileName,
                databaseSchemaInJsonFormat);
        }
    }
}
