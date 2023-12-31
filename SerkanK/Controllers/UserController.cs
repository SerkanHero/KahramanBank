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
        ICardService cardService;

        public UserController(IUserService _userService, ICardService _cardService, IAccountService _accountService, ITransactionService _transactionService)
        {
            userService = _userService;
            accountService = _accountService;
            transactionService = _transactionService;
            cardService = _cardService;
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
                return RedirectToAction("Index", "Home");
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

        [HttpPost]
        public IActionResult AddCard(string accountID)
        {
            if(accountID == null && accountID?.Length < 1)
            {
                accountID = "-1";
            }
            int aID = int.Parse(accountID);

            if (HttpContext.Session.GetString("SessionID") == null)
            {
                TempData["Error"] = "User information is empty.";
                return RedirectToAction("Index", "Home");
            }

            User activeUser = userService.GetUser(Int32.Parse(HttpContext.Session.GetString("SessionID") ?? "-1"));

            int _userID = activeUser.ID;

            if (_userID == -1)
            {
                return RedirectToAction("Logout");
            }

            cardService.AddCard(_userID, aID, 0);
            return RedirectToAction("Cards");
        }

        [HttpGet]
        public IActionResult AddAccount(string userID, string accountType = "0") // 0 is TL Account.
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                TempData["Error"] = "User information is empty.";
                return RedirectToAction("Index", "Home");
            }

            User activeUser = userService.GetUser(Int32.Parse(HttpContext.Session.GetString("SessionID") ?? "-1"));

            int _userID = activeUser.ID;
            int _accountType = Int32.Parse(accountType);

            _accountType = (_accountType == null) ? 0 : _accountType;

            if(_userID == -1 || _userID == null)
            {
                return RedirectToAction("Logout");
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
            int accountID = accountService.GetAccountsOfUserID(activeUser.ID).FirstOrDefault<Account>()?.AccountID ?? -1;
            List<Transaction> transactions = new List<Transaction>();
            if (accountID != -1) {
                transactions = transactionService.GetAllTransactionOfThisAccount(accountID).ToList<Transaction>();
            }
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
            List<Card> Cards = cardService.GetCardWithHolder(activeUser.ID);
            ViewBag.UserInfo = activeUser;
            ViewBag.Cards = Cards;
            ViewBag.Accounts = accountService.GetAccountsOfUserID(activeUser.ID);
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

            int accountID = accountService.GetAccountsOfUserID(activeUser.ID).FirstOrDefault<Account>()?.AccountID ?? -1;
            List<Transaction> transactions = new List<Transaction>();
            if (accountID != -1)
            {
                transactions = transactionService.GetAllTransactionOfThisAccount(accountID).ToList<Transaction>();
            }
            ViewBag.UserInfo = activeUser;
            ViewBag.Transactions = transactions;

            return View();
        }

        public IActionResult MakeTransfer()
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                TempData["Error"] = "User information is empty.";
                return RedirectToAction("Index", "Home");
            }

            User activeUser = userService.GetUser(Int32.Parse(HttpContext.Session.GetString("SessionID") ?? "-1"));
            if (activeUser == null)
            {
                TempData["Error"] = "User information is empty.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.UserInfo = activeUser;
            return View();
        }

        [HttpPost]
        public IActionResult MakeTransfer(int accountID, decimal amount, string IBAN, string typeSelection, string transferNote)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                TempData["Error"] = "User information is empty.";
                return RedirectToAction("Index", "Home");
            }
            User activeUser = userService.GetUser(Int32.Parse(HttpContext.Session.GetString("SessionID") ?? "-1"));
            if (activeUser == null)
            {
                TempData["Error"] = "User information is empty.";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.UserInfo = activeUser;

            Account foundAccount = activeUser.Accounts.FirstOrDefault(account => account.AccountID == accountID) ?? null;

            if(foundAccount == null)
            {
                TempData["Error"] = "Account can not be found.";
                return RedirectToAction("Inside");
            }

            if (foundAccount.Balance >= amount)
            {
                Account rec = accountService.GetAccount(IBAN);
                if (rec == null)
                {
                    return RedirectToAction("Inside");
                }
                transactionService.CreateTransaction(foundAccount.AccountID, rec.AccountID, Int32.Parse(amount.ToString() ?? "-1"), DateTime.Now, transferNote);
            }

            return RedirectToAction("Inside");
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

        public IActionResult Payment()
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
