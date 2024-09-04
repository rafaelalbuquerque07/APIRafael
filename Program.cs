using APIRafael.Data; // Namespace onde o DbContext foi definido
using APIRafael.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Adicionando servi�os ao container
builder.Services.AddRazorPages(); // Inclui suporte para Razor Pages
builder.Services.AddControllersWithViews(); // Inclui suporte para controllers e views
builder.Services.AddSwaggerGen(); // Adiciona Swagger para documenta��o da API

// Configura��o do Entity Framework Core com a string de conex�o
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 23)))); // Configure a vers�o do MySQL

// Configurando a inje��o de depend�ncias
builder.Services.AddScoped<StudentsService>(); // Adiciona StudentsService para inje��o de depend�ncias

var app = builder.Build();

// Configurando o pipeline de requisi��es
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Configura o tratamento de exce��es para produ��o
    app.UseHsts(); // Configura HSTS para seguran�a
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

// Rotas espec�ficas para controladores MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=StudentList}/{id?}");

app.Run();
