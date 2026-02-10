using ClothingStoreMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// підключення до PostgreSQL
builder.Services.AddDbContext<ClothingStoreContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ClothingStoreContext")
    )
);

var app = builder.Build();
app.UseRouting();
app.MapDefaultControllerRoute();
app.Run();

using var context = new ClothingStoreContext(
    new DbContextOptionsBuilder<ClothingStoreContext>()
        .UseNpgsql("Host=localhost;Database=ClothingStoreDb;Username=postgres;Password=твій_пароль")
        .Options
);

try
{
    if (context.Database.CanConnect())
        Console.WriteLine("Підключення успішне!");
    else
        Console.WriteLine("Підключення не вдалося!");
}
catch (Exception ex)
{
    Console.WriteLine("Помилка підключення: " + ex.Message);
}
