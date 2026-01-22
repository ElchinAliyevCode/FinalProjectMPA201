using System.Threading.Tasks;
using FinalProjectMPA201.Contexts;
using FinalProjectMPA201.Helpers;
using FinalProjectMPA201.Models;
using FinalProjectMPA201.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMPA201.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles ="Admin")]
public class EmployeeController : Controller
{
    private readonly FinalDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _folderPath;
    public EmployeeController(FinalDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _folderPath = Path.Combine(_environment.WebRootPath, "images");
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _context.Employees.Select(x => new EmployeeGetVM()
        {
            Id = x.Id,
            Name = x.Name,
            ImagePath = x.ImagePath,
            PositionName = x.Position.Name
        }).ToListAsync();
        return View(employees);
    }

    public async Task<IActionResult> Create()
    {
        await SendPositionsWithViewBag();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateVM vm)
    {
        await SendPositionsWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existPosition = await _context.Positions.AnyAsync(x => x.Id == vm.PositionId);
        if (!existPosition)
        {
            ModelState.AddModelError("", "Position not found");
            return View(vm);
        }

        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("", "Max 2 mb");
            return View(vm);
        }

        if (!vm.Image.CheckType("image"))
        {
            ModelState.AddModelError("", "Image must be in corrrect format");
            return View(vm);
        }

        var uniqueName = await vm.Image.UploadFileAsync(_folderPath);

        Employee employee = new Employee()
        {
            Name = vm.Name,
            ImagePath = uniqueName,
            PositionId = vm.PositionId
        };

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        EmployeeUpdateVM vm = new EmployeeUpdateVM()
        {
            Id = employee.Id,
            Name = employee.Name,
            PositionId = employee.PositionId
        };

        await SendPositionsWithViewBag();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(EmployeeUpdateVM vm)
    {
        await SendPositionsWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == vm.Id);
        if (employee == null)
        {
            return NotFound();
        }

        var existPosition = await _context.Positions.AnyAsync(x => x.Id == vm.PositionId);
        if (!existPosition)
        {
            ModelState.AddModelError("", "Position not found");
            return View(vm);
        }

        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("", "Max 2 mb");
            return View(vm);
        }

        if (!vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("", "Image must be in corrrect format");
            return View(vm);
        }

        employee.Name = vm.Name;
        employee.PositionId = vm.PositionId;

        if (vm.Image is { })
        {
            var uniqueName = await vm.Image.UploadFileAsync(_folderPath);
            var deletedPath = Path.Combine(_folderPath, employee.ImagePath);
            FileHelper.DeleteFile(deletedPath);
            employee.ImagePath = uniqueName;
        }

        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        var deletedPath = Path.Combine(_folderPath, employee.ImagePath);
        FileHelper.DeleteFile(deletedPath);

        return RedirectToAction(nameof(Index));
    }

    private async Task SendPositionsWithViewBag()
    {
        var positions = await _context.Positions.ToListAsync();
        ViewBag.Positions = positions;
    }
}
