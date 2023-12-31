using SerkanK.Models;
using SerkanK.Repository;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace SerkanK.Services
{

    public interface ITransactionService
    {
        public bool CreateTransaction(int senderAccountID, Account senderAccount, int receiverAccountID, Account receiverAccount, int amount, DateTime dateTime, string desc);
        public bool CreateTransaction(int senderAccountID, int receiverAccountID, int amount, DateTime dateTime, string desc);
        public bool dbCheck();
        public Transaction? GetTransaction(int ID);
        public List<Transaction> GetTransactions();
        public ICollection<Transaction> GetAllTransactionOfThisAccountAsSender(int SenderAccountID);
        public ICollection<Transaction> GetAllTransactionOfThisAccountAsReceiver(int ReceiverAccountID);
        public ICollection<Transaction> GetAllTransactionOfThisAccount(int AccountID);
    }

    public class TransactionService : ITransactionService
    {
        ITransactionRepository transactionRepository;
        IAccountService accountService;
        IAccountRepository accountRepository;
        IUserRepository userRepository;

        public TransactionService(ITransactionRepository _transactionRepository, IAccountRepository _accountRepository, IUserRepository _userRepository, IAccountService _accountService)
        {
            transactionRepository = _transactionRepository;
            accountService = _accountService;
            accountRepository = _accountRepository;
            userRepository = _userRepository;
        }


        public List<Transaction> GetTransactions()
        {
            List<Transaction> transactions = transactionRepository.GetTransactions();

            foreach (var item in transactions)
            {
                item.SenderAccount = accountService.GetAccount(item.SenderAccountID);
                item.ReceiverAccount = accountService.GetAccount(item.ReceiverAccountID);
            }

            return transactions;
        }

        public bool CreateTransaction(int senderAccountID, Account senderAccount, int receiverAccountID, Account receiverAccount, int amount, DateTime dateTime, string desc)
        {
            return transactionRepository.CreateTransaction(senderAccountID, senderAccount, receiverAccountID, receiverAccount, amount, dateTime, desc);
        }

        public bool CreateTransaction(int senderAccountID, int receiverAccountID, int amount, DateTime dateTime, string desc)
        {
            Account senderAC = accountRepository.GetAccount(senderAccountID);
            Account receiverAC = accountRepository.GetAccount(receiverAccountID);
            if(senderAC == null || receiverAC == null)
            {
                return false;
            }

            senderAC.Balance -= amount;
            if(senderAC.Balance < 0)
            {
                senderAC.Balance += amount;
                return false;
            }

            receiverAC.Balance += amount;
            return transactionRepository.CreateTransaction(senderAccountID, receiverAccountID, amount, dateTime, desc);
        }

        public Transaction? GetTransaction(int ID)
        {
            Transaction t = transactionRepository.GetTransaction(ID);
            if(t == null)
            {
                return t;
            }
            t.SenderAccount = accountService.GetAccount(t.SenderAccountID) ?? null;
            t.ReceiverAccount = accountService.GetAccount(t.ReceiverAccountID) ?? null;
            return transactionRepository.GetTransaction(ID);
        }

        public ICollection<Transaction> GetAllTransactionOfThisAccountAsSender(int SenderAccountID)
        {
            var transactionList = transactionRepository.GetAllTransactionOfThisAccountAsSender(SenderAccountID);
            foreach (var transactionItem in transactionList)
            {
                transactionItem.SenderAccount = accountRepository.GetAccount(transactionItem.SenderAccountID);
                transactionItem.ReceiverAccount = accountRepository.GetAccount(transactionItem.ReceiverAccountID);
            }
            return transactionList;
        }

        public ICollection<Transaction> GetAllTransactionOfThisAccountAsReceiver(int ReceiverAccountID)
        {
            var transactionList = transactionRepository.GetAllTransactionOfThisAccountAsReceiver(ReceiverAccountID);
            foreach (var transactionItem in transactionList)
            {
                transactionItem.SenderAccount = accountRepository.GetAccount(transactionItem.SenderAccountID);
                transactionItem.ReceiverAccount = accountRepository.GetAccount(transactionItem.ReceiverAccountID);
            }
            return transactionList;
        }

        public ICollection<Transaction> GetAllTransactionOfThisAccount(int AccountID)
        {
            var transactionList = transactionRepository.GetAllTransactionOfThisAccount(AccountID);

            foreach (var transactionItem in transactionList)
            {
                transactionItem.SenderAccount = accountService.GetAccount(transactionItem.SenderAccountID);
                transactionItem.ReceiverAccount = accountService.GetAccount(transactionItem.ReceiverAccountID);
            }
            return transactionList;
        }

        public bool dbCheck() => transactionRepository.dbCheck();

    }
}
