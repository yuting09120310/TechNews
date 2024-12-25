using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechNews.Areas.BackEnd.Models;

namespace TechNews.Areas.BackEnd.Controllers
{
    [Area("BackEnd")]
    public class HomeController : GenericController
    {
        public readonly TechNewsDBContext _context;

        public HomeController(TechNewsDBContext context) : base(context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            GetMenu();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
