using SerkanK.Models;
using SerkanK.Repository;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace SerkanK.Services
{

    public interface IAccountService
    {
        public bool AddAccount(int UserID, int AccountType);
        public Account GetAccount(int id);
        public Account GetAccount(string IBAN);
        public ICollection<Account> GetAccountsOfUserID(int UserID);
        public bool dbCheck();
        public User GetUserFromAccount(Account account);
    }

    public class AccountService : IAccountService
    {

        IAccountRepository accountRepository;
        IUserRepository userRepository;

        public AccountService(IAccountRepository _accountRepository, IUserRepository _userRepository)
        {
            accountRepository = _accountRepository;
            userRepository = _userRepository;
        }

        public bool AddAccount(int UserID, int AccountType)
        {
            accountRepository.AddAccount(UserID, AccountType);
            return true;
        }
        public Account GetAccount(int id)
        {
            Account tempAccount = accountRepository.GetAccount(id);
            tempAccount.User = userRepository.GetUser(tempAccount.UserID);
            return tempAccount;
        }

        public Account GetAccount(string IBAN)
        {
            return accountRepository.GetAccount(IBAN);
        }

        public ICollection<Account> GetAccountsOfUserID(int userID) => accountRepository.GetAccountsOfUser(userID);
        public bool dbCheck() => accountRepository.dbCheck();
        public User GetUserFromAccount(Account account)
        {
            User user = userRepository.GetUser(account.UserID);
            user.Accounts = GetAccountsOfUserID(account.UserID);
            return user;
        }

    }
}
