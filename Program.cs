using APIRafael.Repositories;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços ao container
builder.Services.AddRazorPages(); // Inclui suporte para Razor Pages
builder.Services.AddControllersWithViews(); // Inclui suporte para controllers e views
builder.Services.AddSwaggerGen(); // Adiciona Swagger para documentação da API

// Configurando a injeção de dependências
builder.Services.AddSingleton<StudentRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    var logger = provider.GetRequiredService<ILogger<StudentRepository>>(); // Obtém o logger
    return new StudentRepository(connectionString, logger); // Passa o logger para o repositório
});

var app = builder.Build();

// Configurando o pipeline de requisições
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Configura o tratamento de exceções para produção
    app.UseHsts(); // Configura HSTS para segurança
}
else
{
    app.UseSwagger(); // Configura o Swagger UI no modo desenvolvimento
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages(); // Mapeia as Razor Pages
app.MapControllers(); // Mapeia os controllers da API

// Rotas específicas para controladores MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=StudentList}/{id?}");

app.Run();
