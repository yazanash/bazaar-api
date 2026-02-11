using Bazaar.Entityframework.Models;
using Microsoft.AspNetCore.Identity;

namespace Bazaar.app.HostBuilders
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
          

                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            try
            {
                string[] roleNames = { "Admin", "User", "Tester" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                var adminEmails = configuration.GetSection("InitialSetup:Admins").Get<List<string>>();

                if (adminEmails != null && adminEmails.Any())
                {
                    foreach (var email in adminEmails)
                    {
                        var user = await userManager.FindByEmailAsync(email);

                        if (user == null)
                        {
                            var newAdmin = new AppUser
                            {
                                UserName = email.Split('@')[0],
                                Email = email,
                                EmailConfirmed = true,
                                CreatedAt = DateTime.UtcNow
                            };

                            var result = await userManager.CreateAsync(newAdmin);

                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(newAdmin, "Admin");
                            }
                        }
                        else
                        {
                            if (!await userManager.IsInRoleAsync(user, "Admin"))
                            {
                                await userManager.AddToRoleAsync(user, "Admin");
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
