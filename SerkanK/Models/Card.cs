using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SerkanK.Models
{
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardID { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public int CVV { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public bool IsContactlessPaymentEnabled { get; set; }

        [Required]
        public bool IsOnlineShoppingEnabled { get; set; }

        [ForeignKey("User")]
        public int CardHolderID { get; set; }

        [NotMapped]
        public User CardHolder { get; set; }

        [ForeignKey("Account")]
        public int CardAccountID { get; set; }

        [NotMapped]
        public Account CardAccount { get; set; }

    }
}
