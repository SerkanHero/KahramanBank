using SerkanK.Database;
using SerkanK.Models;

namespace SerkanK.Repository
{

    public interface IUserRepository
    {
        public bool IsUserExists(int ID);
        public bool IsUserExists(string IdentificationNumber);
        public bool IsUserActive(int ID);
        public User? GetUser(int ID);
        public User? GetUser(string IdentificationNumber);
        public bool AddUser(User user);
        public bool dbCheck();
    }

    public class UserRepository : IUserRepository
    {
        SystemDBContext context;

        public UserRepository(SystemDBContext _context)
        {
            context = _context;
            context.StartUp();
        }

        public bool dbCheck() => context.StartUpCheck;
        public bool IsUserExists(int ID) => context.Users.Any(u => u.ID == ID);

        public bool IsUserExists(string IdentificationNumber) => context.Users.Any(u => u.IdentificationNumber == IdentificationNumber);

        public bool IsUserActive(int ID) => context.Users.Any(u => u.ID == ID && u.Status == Models.Enums.UserStatus.Active);

        public User? GetUser(int ID) => context.Users.FirstOrDefault(u => u.ID == ID);

        public User? GetUser(string IdentificationNumber) => context.Users.FirstOrDefault(u => u.IdentificationNumber == IdentificationNumber);

        public bool AddUser(User user)
        {
            if (this.GetUser(user.IdentificationNumber) != null) return false;
            context.Users.Add(user);
            context.SaveChanges();
            return true;
        }

    }
}
