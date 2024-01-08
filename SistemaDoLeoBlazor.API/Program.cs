using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SistemaDoLeoBlazor.API.Context;
using SistemaDoLeoBlazor.API.Repositories.CategoriaRepository;
using SistemaDoLeoBlazor.API.Repositories.ClienteRepository;
using SistemaDoLeoBlazor.API.Repositories.FormaPgtoRepository;
using SistemaDoLeoBlazor.API.Repositories.OperadorRepository;
using SistemaDoLeoBlazor.API.Repositories.PedidoRepository;
using SistemaDoLeoBlazor.API.Repositories.ProdutoRepository;
using SistemaDoLeoBlazor.API.Repositories.ProximoRegistroRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IOperadorRepository, OperadorRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IFormaPgtoRepository, FormaPgtoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IProximoRegistroRepository, ProximoRegistroRepository>();

var app = builder.Build();

app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7161", "https://localhost:7161")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithHeaders(HeaderNames.ContentType)
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
