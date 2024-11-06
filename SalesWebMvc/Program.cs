using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;

var builder = WebApplication.CreateBuilder(args);

// Obtenha a ConnectionString do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("SalesWebMvcContext");

// Registre o DbContext com a ConnectionString
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 40))));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Obtenha o ambiente de execução
var environment = builder.Environment;

// Register SeedingService and any other services you need
builder.Services.AddScoped<SeedingService>();

var app = builder.Build();

// Execute o seeding apenas em ambiente de desenvolvimento
if (environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
        seedingService.Seed();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
