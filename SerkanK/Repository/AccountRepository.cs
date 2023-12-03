using SerkanK.Database;
using SerkanK.Models;

namespace SerkanK.Repository
{

    public interface IAccountRepository
    {
        public bool IsAccountExists(int ID);
        public Account? GetAccount(int ID);
        public bool AddAccount(int UserID, int AccountType);
        public ICollection<Account> GetAccountsOfUser(int UserID);
        public bool dbCheck();
    }

    public class AccountRepository : IAccountRepository
    {
        SystemDBContext context;

        public AccountRepository(SystemDBContext _context)
        {
            context = _context;
            context.StartUp();
        }

        public bool dbCheck() => context.StartUpCheck;
        public bool IsAccountExists(int ID) => context.Accounts.Any(u => u.AccountID == ID);
        public Account? GetAccount(int ID) => context.Accounts.FirstOrDefault(u => u.AccountID == ID);
        public bool AddAccount(int UserID, int AccountType)
        {
            Account newAccount = new Account();
            newAccount.UserID = UserID;
            newAccount.AccountName = AccountType == 0 ? "TL" : "?";


            context.Accounts.Add(newAccount);
            context.SaveChanges();
            return true;
        }
        public ICollection<Account> GetAccountsOfUser(int UserID)
        {
            var AccountsList = context.Accounts.Where(a => a.UserID == UserID).ToList<Account>();
            return AccountsList;
        }
    }
}
