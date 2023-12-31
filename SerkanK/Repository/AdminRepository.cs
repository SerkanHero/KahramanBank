using SerkanK.Database;
using SerkanK.Models;

namespace SerkanK.Repository
{

    public interface IAdminRepository
    {
        public Admin? GetAdmin(int ID);
        public Admin? GetAdmin(string email);
        public bool SetAdmin(string email, string password);
        public bool dbCheck();
    }

    public class AdminRepository : IAdminRepository
    {
        SystemDBContext context;

        public AdminRepository(SystemDBContext _context)
        {
            context = _context;
            context.StartUp();
        }

        public bool dbCheck() => context.StartUpCheck;
        public Admin? GetAdmin(int ID) => context.Admins.FirstOrDefault(u => u.ID == ID);
        public Admin? GetAdmin(string email) => context.Admins.FirstOrDefault(u => u.Email == email);

        public bool SetAdmin(string email, string password)
        {
            Admin A = new Admin(email, password);

            context.Admins.Add(A);
            context.SaveChanges();
            return true;
        }
    }
}
