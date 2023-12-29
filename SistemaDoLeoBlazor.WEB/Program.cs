using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SistemaDoLeoBlazor.WEB;
using SistemaDoLeoBlazor.WEB.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseURL = "https://localhost:7238";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(baseURL)
});

builder.Services.AddScoped<IOperadorService, OperadorService>();

await builder.Build().RunAsync();
