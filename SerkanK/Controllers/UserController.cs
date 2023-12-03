using Microsoft.AspNetCore.Mvc;
using SerkanK.Models;
using SerkanK.Services;
using System.Diagnostics;

namespace SerkanK.Controllers
{
    public class UserController : Controller
    {
        IUserService userService;
        IAccountService accountService;
        ITransactionService transactionService;

        public UserController(IUserService _userService, IAccountService _accountService, ITransactionService _transactionService)
        {
            userService = _userService;
            accountService = _accountService;
            transactionService = _transactionService;
        }

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("SessionID") != null)
            {
                return RedirectToAction("Inside");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if(user == null || user.IdentificationNumber == null || user.Email == null || user.Password == null)
            {
                ViewBag.Error = "User info cannot be empty.";
                return View("~/Views/Home/Index.cshtml");
            }

            if (user.IdentificationNumber.Length != 11)
            {
                ViewBag.Error = "IdentificationNumber must consist of 11 characters.";
                return View("~/Views/Home/Index.cshtml");
            }

            if(userService.Register(user))
                return View("~/Views/Home/Index.cshtml");
            else
            {
                ViewBag.Error = "Failed to create user account.";
                return View("~/Views/Home/Index.cshtml");
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(string inputID, string inputPassword)
        {
            if(inputID == null || inputPassword == null)
            {
                TempData["Error"] = "Identification Number or Password cannot be empty.";
                return RedirectToAction("Index", "Home");
            }

            User bankUser = userService.Login(inputID, inputPassword);
            if(bankUser == null)
            {
                TempData["Error"] = "Login information is incorrect.";
                return RedirectToAction("Index", "Home");
            }

            HttpContext.Session.SetString("SessionID", bankUser.ID.ToString());
            return RedirectToAction("Inside");
        }

        [HttpGet]
        public IActionResult AddAccount(string userID, string accountType = "0") // 0 is TL Account.
        {
            Debug.WriteLine("gelen: " + userID);
            int _userID = Int32.Parse(userID);
            int _accountType = Int32.Parse(accountType);

            if (_accountType == null)
            {
                _accountType = 0;
            }

            if(_userID == null)
            {
                if (HttpContext.Session.GetString("SessionID") != null)
                {
                    HttpContext.Session.Remove("SessionID");
                }
                TempData["Error"] = "Login information is incorrect.";
                return RedirectToAction("Index", "Home");
            }

            accountService.AddAccount(_userID, _accountType);

            return RedirectToAction("Inside");
        }

        [HttpPost]
        public IActionResult FastTransfer(string accountID, string targetAccountID, string amount, string transferNote = "-")
        {
            if(accountService.GetAccount(Int32.Parse(accountID)) != null && accountService.GetAccount(Int32.Parse(targetAccountID)) != null)
            {
                transactionService.CreateTransaction(Int32.Parse(accountID), Int32.Parse(targetAccountID), Int32.Parse(amount), DateTime.Now, transferNote);
                return RedirectToAction("Inside");
            }
            else
            {
                ViewBag.Error = "Please use an existing account ID (for receiver)";
                return View();
            }

        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("SessionID") != null)
            {
                HttpContext.Session.Remove("SessionID");
            }

            return RedirectToAction("Login");
        }

        public IActionResult Inside()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                TempData["Error"] = "User information is empty.";
                return RedirectToAction("Index", "Home");
            }

            User activeUser = userService.GetUser(Int32.Parse(HttpContext.Session.GetString("SessionID")));
            List<Transaction> transactions = transactionService.GetAllTransactionOfThisAccount(accountService.GetAccountsOfUserID(activeUser.ID).First<Account>().AccountID).ToList<Transaction>();
            ViewBag.UserInfo = activeUser;
            ViewBag.Transactions = transactions;
            return View();
        }

        public IActionResult Cards()
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

        public IActionResult Transfers()
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

        public IActionResult MakeTransfer()
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

        public IActionResult Accounts()
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
