using APIRafael.Data; // Namespace onde o DbContext foi definido
using APIRafael.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços ao container
builder.Services.AddRazorPages(); // Inclui suporte para Razor Pages
builder.Services.AddControllersWithViews(); // Inclui suporte para controllers e views
builder.Services.AddSwaggerGen(); // Adiciona Swagger para documentação da API

// Configuração do Entity Framework Core com a string de conexão
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 23)))); // Configure a versão do MySQL

// Configurando a injeção de dependências
builder.Services.AddScoped<StudentsService>(); // Adiciona StudentsService para injeção de dependências

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
