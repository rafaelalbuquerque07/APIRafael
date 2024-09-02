using APIRafael.Repositories;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Adicionando servi�os ao container
builder.Services.AddRazorPages(); // Inclui suporte para Razor Pages
builder.Services.AddControllersWithViews(); // Inclui suporte para controllers e views
builder.Services.AddSwaggerGen(); // Adiciona Swagger para documenta��o da API

// Configurando a inje��o de depend�ncias
builder.Services.AddSingleton<StudentRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    var logger = provider.GetRequiredService<ILogger<StudentRepository>>(); // Obt�m o logger
    return new StudentRepository(connectionString, logger); // Passa o logger para o reposit�rio
});

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
