using APIRafael.Repositories; // Certifique-se de importar o namespace correto
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços ao container
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(); // Adiciona Swagger para documentação da API

// Configurando a injeção de dependências
builder.Services.AddSingleton<StudentRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new StudentRepository(connectionString);
});

var app = builder.Build();

// Configurando o pipeline de requisições
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Configura o Swagger UI
}

app.UseAuthorization();

app.MapControllers();

app.Run();
