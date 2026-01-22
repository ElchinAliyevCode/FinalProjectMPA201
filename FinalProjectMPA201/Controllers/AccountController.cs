using System.Threading.Tasks;
using FinalProjectMPA201.Models;
using FinalProjectMPA201.ViewModels.AcoountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMPA201.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existEmail = await _userManager.FindByEmailAsync(vm.Email);
        if (existEmail != null)
        {
            ModelState.AddModelError("", "Email is already exist");
            return View(vm);
        }

        var existUser = await _userManager.FindByNameAsync(vm.UserName);
        if (existUser != null)
        {
            ModelState.AddModelError("", "Username is already exist");
            return View(vm);
        }

        AppUser user = new AppUser()
        {
            Email = vm.Email,
            UserName = vm.UserName
        };

        var result = await _userManager.CreateAsync(user, vm.Password);
        if (!result.Succeeded)
        {
            return BadRequest();
        }

        result = await _userManager.AddToRoleAsync(user, "Member");
        if (!result.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return BadRequest();
        }

        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var user = await _userManager.FindByEmailAsync(vm.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Email or password is wrong");
            return View(vm);
        }

        var result = await _userManager.CheckPasswordAsync(user, vm.Password);
        if (!result)
        {
            ModelState.AddModelError("", "Email or password is wrong");
            return View(vm);
        }

        await _signInManager.SignInAsync(user, false);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
