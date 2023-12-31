using Microsoft.AspNetCore.Mvc;
using SerkanK.Models;
using SerkanK.Services;
using System.Diagnostics;

namespace SerkanK.Controllers
{
    public class AdminController : Controller
    {
        IUserService userService;
        IAccountService accountService;
        ITransactionService transactionService;
        ICardService cardService;
        IAdminService adminService;

        public AdminController(IUserService _userService, IAdminService _adminService, IAccountService _accountService, ITransactionService _transactionService)
        {
            userService = _userService;
            accountService = _accountService;
            transactionService = _transactionService;
            adminService = _adminService;
        }

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("AdminSessionID") != null)
            {
                return RedirectToAction("Inside");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if(email == null || password == null)
            {
                TempData["Error"] = "Identification Number or Password cannot be empty.";
                return RedirectToAction("Index", "Home");
            }

            Admin admin = adminService.Login(email, password);
            if(admin == null)
            {
                TempData["Error"] = "Login information is incorrect.";
                return RedirectToAction("Index", "Home");
            }

            HttpContext.Session.SetString("AdminSessionID", admin.ID.ToString());
            return RedirectToAction("Inside");
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("AdminSessionID") != null)
            {
                HttpContext.Session.Remove("AdminSessionID");
            }

            return RedirectToAction("Login");
        }

        public IActionResult Inside()
        {
            if (HttpContext.Session.GetString("AdminSessionID") == null)
            {
                return RedirectToAction("Logout");
            }

            Admin admin = adminService.GetAdmin(Int32.Parse(HttpContext.Session.GetString("AdminSessionID")));
            if (admin == null)
            {
                return RedirectToAction("Logout");
            }

            ViewBag.Users = userService.GetUsers();
            ViewBag.Transfers = transactionService.GetTransactions();
            return View();
        }

        public IActionResult Support()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                TempData["Error"] = "User information is empty.";
                return RedirectToAction("Index", "Home");
            }

            User activeUser = userService.GetUser(Int32.Parse(HttpContext.Session.GetString("SessionID")));
            ViewBag.UserInfo = activeUser;
            return View();
        }

    }
}
