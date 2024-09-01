using APIRafael.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Adicionando serviços ao container
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(); // Adiciona Swagger para documentação da API

// Configurando a injeção de dependências
builder.Services.AddSingleton<StudentRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    var logger = provider.GetRequiredService<ILogger<StudentRepository>>();
    return new StudentRepository(connectionString, logger);
});

// Configurando o pipeline de requisições
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Configura o Swagger UI
}

app.UseAuthorization();

app.MapControllers();

app.Run();
