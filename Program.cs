using APIRafael.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Adicionando servi�os ao container
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(); // Adiciona Swagger para documenta��o da API

// Configurando a inje��o de depend�ncias
builder.Services.AddSingleton<StudentRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    var logger = provider.GetRequiredService<ILogger<StudentRepository>>();
    return new StudentRepository(connectionString, logger);
});

var app = builder.Build();

// Configurando o pipeline de requisi��es
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Configura o Swagger UI
}

app.UseAuthorization();

app.MapControllers();

app.Run();
