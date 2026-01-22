using System.Diagnostics;
using System.Threading.Tasks;
using FinalProjectMPA201.Contexts;
using FinalProjectMPA201.Models;
using FinalProjectMPA201.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMPA201.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinalDbContext _context;

        public HomeController(FinalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Select(x => new EmployeeGetVM()
            {
                Name=x.Name,
                ImagePath=x.ImagePath,
                PositionName=x.Position.Name
            }).ToListAsync();
            return View(employees);
        }


    }
}
