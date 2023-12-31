using SerkanK.Models;
using SerkanK.Repository;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace SerkanK.Services
{

    public interface IAdminService
    {
        public Admin? Login(string email, string password);
        public bool Register(Admin a);
        public Admin? GetAdmin(int ID);
        public Admin? GetAdmin(string email);
    }

    public class AdminService : IAdminService
    {

        IAccountRepository accountRepository;
        IUserRepository userRepository;
        IAdminRepository adminRepository;

        public AdminService(IAccountRepository _accountRepository, IUserRepository _userRepository, IAdminRepository _adminRepository)
        {
            accountRepository = _accountRepository;
            userRepository = _userRepository;
            adminRepository = _adminRepository;
        }

        public Admin? GetAdmin(int ID) => adminRepository.GetAdmin(ID);

        public Admin? GetAdmin(string email) => adminRepository.GetAdmin(email);

        public Admin? Login(string email, string password)
        {
            Admin admin = adminRepository.GetAdmin(email);
            if(admin == null)
            {
                return null;
            }

            if(admin.Password != password)
            {
                return null;
            }

            return admin;
        }

        public bool Register(Admin a)
        {
            return adminRepository.SetAdmin(a.Email, a.Password);
        }
    }
}
