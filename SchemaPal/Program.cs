using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchemaPal;
using SchemaPal.Services.HelperServices;
using SchemaPal.Services.SchemaMakerServices;
using SchemaPal.Services.UserServices;

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
builder.Services.AddSingleton<IJsonConverter, JsonConverter>();
builder.Services.AddSingleton<IComponentActionsStorage, ComponentActionsStorage>();

builder.Services.AddScoped<ISchemaPalApiService, SchemaPalApiService>();
builder.Services.AddScoped<ISchemaInjectionService, SchemaInjectionService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IResultProcessor, ResultProcessor>();

builder.Services.AddTransient<ISchemaObjectFactory, SchemaObjectFactory>();

await builder.Build().RunAsync();
