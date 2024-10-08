using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchemaPal;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using SchemaPal.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddSingleton<ISchemaObjectFactory, SchemaObjectFactory>();
builder.Services.AddSingleton<IPositionService, PositionService>();
builder.Services.AddSingleton<ICoordinatesCalculator, CoordinatesCalculator>();

await builder.Build().RunAsync();
