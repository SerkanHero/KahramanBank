using SerkanK.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SerkanK.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        [ForeignKey("User")]
        public int SenderAccountID { get; set; }

        [NotMapped]
        public Account SenderAccount { get; set; }

        [ForeignKey("User")]
        public int ReceiverAccountID { get; set; }

        [NotMapped]
        public Account ReceiverAccount { get; set; }

        [Required]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
