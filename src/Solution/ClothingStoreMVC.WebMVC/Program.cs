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
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");
app.Run();
