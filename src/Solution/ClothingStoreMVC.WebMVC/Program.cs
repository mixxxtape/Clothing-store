using ClothingStoreMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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
        .UseNpgsql("Host=localhost;Database=StoreDb;Username=postgres;Password=dasha2007")
        .Options
);