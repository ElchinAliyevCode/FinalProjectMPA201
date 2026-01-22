using System.Threading.Tasks;
using FinalProjectMPA201.Contexts;
using FinalProjectMPA201.Models;
using FinalProjectMPA201.ViewModels.PositionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMPA201.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles ="Admin")]
public class PositionController : Controller
{
    private readonly FinalDbContext _context;

    public PositionController(FinalDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var positions = await _context.Positions.Select(x => new PositionGetVM()
        {
            Id=x.Id,
            Name=x.Name
        }).ToListAsync();
        return View(positions);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PositionCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Position position = new Position()
        {
            Name = vm.Name
        };

        await _context.Positions.AddAsync(position);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var position = await _context.Positions.FindAsync(id);
        if (position == null)
        {
            return NotFound();
        }

        PositionUpdateVM vm = new PositionUpdateVM()
        {
            Id = position.Id,
            Name = position.Name
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PositionUpdateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var position = await _context.Positions.FirstOrDefaultAsync(x => x.Id == vm.Id);
        if (position == null)
        {
            return NotFound();
        }

        position.Name = vm.Name;
        _context.Positions.Update(position);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Delete(int id)
    {
        var position = await _context.Positions.FindAsync(id);
        if (position == null)
        {
            return NotFound();
        }
        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
