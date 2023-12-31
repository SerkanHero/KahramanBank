using Microsoft.AspNetCore.Mvc;
using SerkanK.Models;
using SerkanK.Services;
using System.Diagnostics;

namespace SerkanK.Controllers
{
    public class ReceiptController : Controller
    {
        IUserService userService;
        IAccountService accountService;
        ITransactionService transactionService;

        public ReceiptController(IUserService _userService, IAccountService _accountService, ITransactionService _transactionService)
        {
            userService = _userService;
            accountService = _accountService;
            transactionService = _transactionService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "User");
        }

        public IActionResult Show(int transferID)
        {
            if (HttpContext.Session.GetString("SessionID") == null)
            {
                TempData["Error"] = "User information is empty.";
                return RedirectToAction("Index", "Home");
            }
            Debug.WriteLine("transferID : " + transferID);

            User activeUser = userService.GetUser(Int32.Parse(HttpContext.Session.GetString("SessionID")));
            Transaction transaction = transactionService.GetTransaction(transferID);

            if (transaction == null || activeUser == null)
            {
                return RedirectToAction("Inside", "User");
            }

            if (
                transaction.ReceiverAccount.AccountID == (activeUser.Accounts.FirstOrDefault()?.AccountID ?? -1)
                ||
                transaction.SenderAccount.AccountID == (activeUser.Accounts.FirstOrDefault()?.AccountID ?? -1)
            )
            {
                ViewBag.UserInfo = activeUser;
                ViewBag.Transaction = transaction;
                return View();
            }
            return RedirectToAction("Inside", "User");
        }
    }
}
