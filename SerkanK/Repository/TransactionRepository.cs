using SerkanK.Database;
using SerkanK.Models;
using System.Linq;

namespace SerkanK.Repository
{

    public interface ITransactionRepository
    {
        public bool CreateTransaction(int senderAccountID, int receiverAccountID, int amount, DateTime dateTime, string desc);
        public bool CreateTransaction(int senderAccountID, Account senderAccount, int receiverAccountID, Account receiverAccount, int amount, DateTime dateTime, string desc);
        public Transaction GetTransaction(int ID);
        public ICollection<Transaction> GetAllTransactionOfThisAccountAsSender(int SenderAccountID);
        public ICollection<Transaction> GetAllTransactionOfThisAccountAsReceiver(int ReceiverAccountID);
        public ICollection<Transaction> GetAllTransactionOfThisAccount(int AccountID);
        public bool dbCheck();
    }

    public class TransactionRepository : ITransactionRepository
    {
        SystemDBContext context;

        public TransactionRepository(SystemDBContext _context)
        {
            context = _context;
            context.StartUp();
        }

        public bool dbCheck() => context.StartUpCheck;
        public bool CreateTransaction(int senderAccountID, Account senderAccount, int receiverAccountID, Account receiverAccount, int amount, DateTime dateTime, string desc)
        {
            Transaction transaction = new Transaction();
            transaction.SenderAccountID = senderAccountID;
            transaction.ReceiverAccountID = receiverAccountID;
            transaction.Amount = amount;
            transaction.Description = desc;
            transaction.TransactionDate = dateTime;
            transaction.SenderAccount = senderAccount;
            transaction.ReceiverAccount = receiverAccount;


            context.Transactions.Add(transaction);
            context.SaveChanges();
            return true;
        }

        public bool CreateTransaction(int senderAccountID, int receiverAccountID, int amount, DateTime dateTime, string desc)
        {
            Transaction transaction = new Transaction();
            transaction.SenderAccountID = senderAccountID;
            transaction.ReceiverAccountID = receiverAccountID;
            transaction.Amount = amount;
            transaction.Description = desc;
            transaction.TransactionDate = dateTime;


            context.Transactions.Add(transaction);
            context.SaveChanges();
            return true;
        }

        public Transaction GetTransaction(int ID)
        {
            return context.Transactions.Where(t => t.TransactionID == ID) as Transaction;
        }

        public ICollection<Transaction> GetAllTransactionOfThisAccountAsSender(int SenderAccountID)
        {
            var transactionList = context.Transactions.Where(t => t.SenderAccountID == SenderAccountID).ToList<Transaction>();
            return transactionList;
        }

        public ICollection<Transaction> GetAllTransactionOfThisAccountAsReceiver(int ReceiverAccountID)
        {
            var transactionList = context.Transactions.Where(t => t.ReceiverAccountID == ReceiverAccountID).ToList<Transaction>();
            return transactionList;
        }

        public ICollection<Transaction> GetAllTransactionOfThisAccount(int AccountID)
        {
            var transactionList = context.Transactions.Where(t => t.SenderAccountID == AccountID).ToList<Transaction>();
            var transactionList2 = context.Transactions.Where(t => t.ReceiverAccountID == AccountID).ToList<Transaction>();
            var transactionListFull = transactionList.Concat(transactionList2).Distinct().ToList();

            return transactionListFull;
        }
    }
}
