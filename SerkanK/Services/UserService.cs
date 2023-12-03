using SerkanK.Models;
using SerkanK.Repository;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace SerkanK.Services
{

    public interface IUserService
    {
        public bool AddUser(User user);
        public User GetUser(int id);
        public User GetUser(User user);
        public User GetUser(string IN);
        public User Login(string IdentificationNumber, string Password);
        public bool Register(User user);
        public bool dbCheck();
    }

    public class UserService : IUserService
    {

        IUserRepository userRepository;
        IAccountRepository accountRepository;

        public UserService(IUserRepository _userRepository, IAccountRepository _accountRepository)
        {
            userRepository = _userRepository;
            accountRepository = _accountRepository;
        }

        public bool AddUser(User user)
        {
            if (user == null ) return false;
            if (userRepository.IsUserExists(user.IdentificationNumber)) return false;

            userRepository.AddUser(user);
            return true;
        }

        public bool dbCheck() => userRepository.dbCheck();

        public User GetUser(int id)
        {
            User tempUser = userRepository.GetUser(id);
            tempUser.Accounts = accountRepository.GetAccountsOfUser(id);
            return tempUser;
        }
        public User GetUser(User user)
        {
            return GetUser(user.ID);
        }
        public User GetUser(string IN)
        {
            User tempUser = userRepository.GetUser(IN);
            tempUser.Accounts = accountRepository.GetAccountsOfUser(tempUser.ID);
            return tempUser;
        }
        public User Login(string IdentificationNumber, string Password)
        {
            User u = userRepository.GetUser(IdentificationNumber);
            if(u != null && u.Password == Password) {
                return u;
            }
            return null;
        }

        public bool Register(User user)
        {
            if (user == null) return false;
            if (user.IdentificationNumber == null || user.Password == null || user.Email == null) return false;
            user.CustomerNumber = "-";
            return userRepository.AddUser(user);
        }

    }
}
