using SerkanK.Database;
using SerkanK.Models;

namespace SerkanK.Repository
{

    public interface IAccountRepository
    {
        public bool IsAccountExists(int ID);
        public Account? GetAccount(int ID);
        public Account? GetAccount(string IBAN);
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

        public string IBANGenerate(int UserID)
        {
            string CountryCode = "TR";
            string ControlCode = "11";
            string BankCode = "23232";

            Random r = new Random();
            int eightDigit = r.Next(10000000, 100000000);
            string Code = UserID.ToString().PadLeft(8, '0') + eightDigit.ToString().PadLeft(8, '1');

            string IBAN = CountryCode + ControlCode + BankCode + "0" + Code;

            return IBAN;
        }

        public bool dbCheck() => context.StartUpCheck;
        public bool IsAccountExists(int ID) => context.Accounts.Any(u => u.AccountID == ID);
        public Account? GetAccount(int ID) => context.Accounts.FirstOrDefault(u => u.AccountID == ID);
        public Account? GetAccount(string IBAN) => context.Accounts.FirstOrDefault(u => u.IBAN == IBAN);
        public bool AddAccount(int UserID, int AccountType)
        {
            Account newAccount = new Account();
            newAccount.UserID = UserID;
            newAccount.IBAN = IBANGenerate(UserID);
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
