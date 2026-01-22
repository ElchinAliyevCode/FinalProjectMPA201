using System.Diagnostics;
using FinalProjectMPA201.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectMPA201.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }


    }
}
