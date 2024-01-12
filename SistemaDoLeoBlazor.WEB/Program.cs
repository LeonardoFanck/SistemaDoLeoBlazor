using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SistemaDoLeoBlazor.WEB;
using SistemaDoLeoBlazor.WEB.Services.CategoriaService;
using SistemaDoLeoBlazor.WEB.Services.CLienteService;
using SistemaDoLeoBlazor.WEB.Services.FormaPgtoService;
using SistemaDoLeoBlazor.WEB.Services.OperadorLocalStorageService;
using SistemaDoLeoBlazor.WEB.Services.OperadorService.OperadorService;
using SistemaDoLeoBlazor.WEB.Services.PedidoService;
using SistemaDoLeoBlazor.WEB.Services.ProdutoService;
using SistemaDoLeoBlazor.WEB.Services.ProximoRegistroService;
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
builder.Services.AddScoped<IProximoRegistroService, ProximoRegistroService>();
builder.Services.AddScoped<ToasterService>();
builder.Services.AddScoped<IOperadorLocalService, OperadorLocalService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IFormaPgtoService, FormaPgtoService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddBlazoredSessionStorage();

await builder.Build().RunAsync();
