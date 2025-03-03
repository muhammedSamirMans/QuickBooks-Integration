using Karage.Domain.Entities;
using Karage.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Karage.APIs.Extensions
{
    public static class ApplicationDbInitializer
    {
        /// <summary>
        /// Applies pending migrations and seeds the default admin user.
        /// </summary>
        public static void ApplyMigrationsAndSeedData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<KarageDBContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Apply Migrations
            dbContext.Database.Migrate();

            // Seed Default Data
            SeedAdminUser(userManager, roleManager).Wait();
        }

        /// <summary>
        /// Seeds the default admin user if it does not exist.
        /// </summary>
        private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            const string adminEmail = "MoSamir@karage.com";
            const string adminPassword = "Mo_Samir_@123";
            const string adminRole = "Admin";

            // Check if admin role exists, create if not
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Check if admin user exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}
