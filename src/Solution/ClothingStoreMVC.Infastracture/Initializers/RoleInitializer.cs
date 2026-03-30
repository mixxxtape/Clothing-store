using ClothingStoreMVC.Domain.Entities.UserAggregates;
using Microsoft.AspNetCore.Identity;

namespace ClothingStoreMVC.Infrastructure.Initializers
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ClothingStoreContext context) 
        {
            string adminEmail = "admin@gmail.com";
            string password = "Admin_134340";

            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));

            if (await roleManager.FindByNameAsync("user") == null)
                await roleManager.CreateAsync(new IdentityRole("user"));

            if (!context.Roles.Any(r => r.Name == "admin"))
            {
                context.Roles.Add(new Role { Name = "admin" });
                await context.SaveChangesAsync();
            }
            if (!context.Roles.Any(r => r.Name == "user"))
            {
                context.Roles.Add(new Role { Name = "user" });
                await context.SaveChangesAsync();
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                AppUser admin = new AppUser
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "+38011037080"
                };

                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");

                    var adminRole = context.Roles.First(r => r.Name == "admin");
                    if (!context.Users.Any(u => u.IdentityUserId == admin.Id))
                    {
                        context.Users.Add(new ClothingStoreMVC.Domain.Entities.UserAggregates.User
                        {
                            IdentityUserId = admin.Id,
                            RoleId = adminRole.Id
                        });
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}