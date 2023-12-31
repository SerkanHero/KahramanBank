using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SerkanK.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }

        [Required]
        public int UserID { get; set; }
        
        [NotMapped]
        public User User { get; set; }

        public string IBAN { get; set; }

        public string AccountName { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}
