using Microsoft.AspNetCore.Mvc;
using SerkanK.Models;
using System.Diagnostics;

namespace SerkanK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("SessionID") != null)
            {
                return RedirectToAction("Index", "User");
            }
            if (TempData["Error"]!=null)
            {
                ViewBag.Error = TempData["Error"].ToString();
            }
            return View();
        }
        public IActionResult StaticPage()
        {
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