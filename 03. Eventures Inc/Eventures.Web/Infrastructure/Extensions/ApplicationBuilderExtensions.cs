namespace Eventures.Web.Infrastructure.Extensions
{
    using Common;
    using Data;
    using Eventures.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<EventuresDbContext>().Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () =>
                    {
                        var admin = WebConstants.AdministratorRole;
                        var user = WebConstants.UserRole;

                        var roles = new[]
                        {
                            admin,
                            user
                        };

                        foreach (var role in roles)
                        {
                            var roleExists = await roleManager.RoleExistsAsync(role);

                            if (!roleExists)
                            {
                                await roleManager.CreateAsync(new IdentityRole
                                {
                                    Name = role
                                });
                            }
                        }

                        var adminEmail = "admin@admin.com";

                        var adminUser = await userManager.FindByEmailAsync(adminEmail);

                        if (adminUser == null)
                        {
                            adminUser = new User
                            {
                                Email = adminEmail,
                                UserName = admin,
                                FirstName = "Teodor",
                                LastName = "Ivanov",
                                UniqueCitizenNumber = "77ADMINCITIZEN77"
                            };

                            await userManager.CreateAsync(adminUser, "admin12");

                            await userManager.AddToRoleAsync(adminUser, admin);
                        }
                    })
                    .Wait();
            }

            return app;
        }
    }
}