using Microsoft.EntityFrameworkCore;
using Proyecto_2_MVC.Data;

var builder = WebApplication.CreateBuilder(args);


// Agregar servicios de sesi�n:

builder.Services.AddDistributedMemoryCache(); // Necesario para almacenar datos de sesi�n en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true; // Cookie accesible solo v�a HTTP
    options.Cookie.IsEssential = true; // Requerido para GDPR
});


// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura la cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}");

app.Run();
