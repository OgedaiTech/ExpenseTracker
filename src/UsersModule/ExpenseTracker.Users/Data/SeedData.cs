using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Users.Data;

public static class SeedData
{
  public static async Task InitiliazeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
  {
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var systemAdmin = "SystemAdmin";
    var tenantAdmin = "TenantAdmin";
    var userRole = "User";

    string[] roles = [systemAdmin, tenantAdmin, userRole];

    foreach (var role in roles)
    {
      if (!await roleManager.RoleExistsAsync(role))
      {
        await roleManager.CreateAsync(new IdentityRole(role));
      }
    }

    // first admin user
    var adminEmail = "admin@expensetracker.com";
    var adminPassword = configuration["AdminUser:Password"];

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
      adminUser = new ApplicationUser
      {
        UserName = adminEmail,
        Email = adminEmail,
        EmailConfirmed = true,
        TenantId = Guid.Empty
      };

      await userManager.CreateAsync(adminUser, adminPassword!);
      await userManager.AddToRoleAsync(adminUser, systemAdmin);
    }
  }
}
