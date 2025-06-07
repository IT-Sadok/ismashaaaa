using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Enums;
using MakeupClone.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;

namespace MakeupClone.Infrastructure.Identity.Seeders;

public class AdminSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly AdminAccountSettings _adminAccountSettings;

    public AdminSeeder(UserManager<User> userManager, AdminAccountSettings adminAccountSettings)
    {
        _userManager = userManager;
        _adminAccountSettings = adminAccountSettings;
    }

    public async Task SeedAsync()
    {
        var existingAdmin = await _userManager.FindByEmailAsync(_adminAccountSettings.Email);
        if (existingAdmin != null)
            return;

        var admin = new User
        {
            Email = _adminAccountSettings.Email,
            UserName = _adminAccountSettings.Email,
            FirstName = "Admin",
            LastName = "Admin"
        };

        var result = await _userManager.CreateAsync(admin, _adminAccountSettings.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(admin, Roles.Admin);
        }
    }
}