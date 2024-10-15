using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchemaPal;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using SchemaPal.Services;
using SchemaPal.Services.SchemaMakerServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("SchemaPalApi", client =>
{
    var apiBaseAddress = builder.Configuration["SchemaPalApiSettings:BaseAddress"];
    client.BaseAddress = new Uri(apiBaseAddress);
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddSingleton<IPositionService, PositionService>();
builder.Services.AddSingleton<ICoordinatesCalculator, CoordinatesCalculator>();
builder.Services.AddSingleton<IStyleService, StyleService>();
builder.Services.AddSingleton<IExportService, ExportService>();
builder.Services.AddSingleton<IJsonService, JsonService>();

builder.Services.AddScoped<ISchemaPalApiService, SchemaPalApiService>();

builder.Services.AddTransient<ISchemaObjectFactory, SchemaObjectFactory>();

await builder.Build().RunAsync();
