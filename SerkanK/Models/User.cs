using SerkanK.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SerkanK.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string IdentificationNumber { get; set; }

        [Required]
        public string CustomerNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EnumDataType(typeof(UserStatus))]
        public UserStatus Status { get; set; }

        [NotMapped]
        public ICollection<Account> Accounts { get; set; }

        public User(){}
        public User(string Name, string Surname, string Email, string Password, string IdentificationNumber, string CustomerNumber, UserStatus status)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.Email = Email;
            this.Password = Password;
            this.IdentificationNumber = IdentificationNumber;
            this.CustomerNumber = CustomerNumber;
            this.Status = status;
        }

        public User(string Name, string Surname, string Email, string Password, string IdentificationNumber = null, string CustomerNumber = null, UserStatus status = UserStatus.Inactive, ICollection<Account> accounts = null)
        {
            this.IdentificationNumber = IdentificationNumber;
            this.CustomerNumber = CustomerNumber;
            this.Name = Name;
            this.Surname = Surname;
            this.Email = Email;
            this.Password = Password;
            this.Status = status;
            Accounts = accounts;
        }

        public User(User user)
        {
            this.ID = user.ID;
            this.CustomerNumber = user.CustomerNumber;
            this.IdentificationNumber = user.IdentificationNumber;
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.Status = user.Status;
            this.Email = user.Email;
            this.Password = user.Password;
            this.Accounts = user.Accounts;
        }

    }
}
