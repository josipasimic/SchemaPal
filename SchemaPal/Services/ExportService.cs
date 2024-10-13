using Microsoft.JSInterop;
using SchemaPal.DataTransferObjects;
using SchemaPal.Helpers.SchemaMakerHelpers;
using System.Text.Json;

namespace SchemaPal.Services
{
    public class ExportService : IExportService
    {
        private const string ExportPngJavaScriptFunctionName = "exportDivToPng";  
        
        private const string ExportJsnJavaScriptFunctionName = "saveAsFile";    
        private const string ExportJsnFileName = "schema.json";    

        private readonly IJSRuntime _jsRuntime;

        public ExportService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ExportSchemaAsPng()
        {
            await _jsRuntime.InvokeVoidAsync(
                ExportPngJavaScriptFunctionName, 
                SchemaMakerConstants.ExportPngDivId);
        }

        public async Task ExportSchemaAsJson(DatabaseSchema databaseSchema)
        {
            var databaseSchemaInJsonFormat = JsonSerializer.Serialize(databaseSchema);

            await _jsRuntime.InvokeVoidAsync(
                ExportJsnJavaScriptFunctionName, 
                ExportJsnFileName, 
                databaseSchemaInJsonFormat);
        }
    }
}
