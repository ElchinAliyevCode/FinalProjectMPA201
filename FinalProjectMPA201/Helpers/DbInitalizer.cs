using System.Threading.Tasks;
using FinalProjectMPA201.Models;
using FinalProjectMPA201.ViewModels.AcoountViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace FinalProjectMPA201.Helpers;

public class DbInitalizer
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AdminVM _admin;

    public DbInitalizer(RoleManager<IdentityRole> roleManager, IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _configuration = configuration;
        _admin = _configuration.GetRequiredSection("AdminSettings").Get<AdminVM>() ?? new();
        _userManager = userManager;
    }

    public async Task CreateRolesAndAdmin()
    {
        await CreateRolesAsync();
        await CreateAdmin();

    }

    private async Task CreateAdmin()
    {
        AppUser admin = new AppUser()
        {
            UserName = _admin.UserName,
            Email = _admin.Email
        };

        var result = await _userManager.CreateAsync(admin, _admin.Password);
    }

    private async Task CreateRolesAsync()
    {
        await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
        await _roleManager.CreateAsync(new IdentityRole() { Name = "Member" });
    }
}
