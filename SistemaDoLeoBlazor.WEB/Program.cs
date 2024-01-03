using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SistemaDoLeoBlazor.WEB;
using SistemaDoLeoBlazor.WEB.Services;
using SistemaDoLeoBlazor.WEB.Toaster;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseURL = "https://localhost:7238";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(baseURL)
});

builder.Services.AddScoped<IOperadorService, OperadorService>();
builder.Services.AddScoped<ToasterService>();

await builder.Build().RunAsync();
