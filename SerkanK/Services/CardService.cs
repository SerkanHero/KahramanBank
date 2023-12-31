using SerkanK.Models;
using SerkanK.Repository;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace SerkanK.Services
{

    public interface ICardService
    {
        public bool AddCard(int UserID, int AccountID, int CardType);
        public Card? GetCard(int id);
        public Card? GetCard(string CardNumber);
        public List<Card> GetCardWithHolder(int CardHolderID);
        public bool IsCardExists(int CardID);
    }

    public class CardService : ICardService
    {

        IAccountRepository accountRepository;
        IUserRepository userRepository;
        ICardRepository cardRepository;

        public CardService(IAccountRepository _accountRepository, IUserRepository _userRepository, ICardRepository _cardRepository)
        {
            accountRepository = _accountRepository;
            userRepository = _userRepository;
            cardRepository = _cardRepository;
        }

        public bool AddCard(int UserID, int AccountID, int CardType)
        {
            return cardRepository.AddCard(UserID, AccountID, CardType);
        }

        public Card? GetCard(int id)
        {
            Card myCard = cardRepository.GetCard(id);
            myCard.CardHolder = userRepository.GetUser(myCard.CardHolderID);
            myCard.CardAccount = accountRepository.GetAccount(myCard.CardAccountID);
            return myCard;
        }

        public Card? GetCard(string CardNumber)
        {
            return GetCard(cardRepository.GetCard(CardNumber)?.CardID ?? -1);
        }

        public List<Card> GetCardWithHolder(int CardHolderID)
        {
            User user = userRepository.GetUser(CardHolderID);
            List<Card> cards = cardRepository.GetCardWithHolder(CardHolderID);
            foreach (var item in cards)
            {
                item.CardHolder = user;
                item.CardAccount = accountRepository.GetAccount(item.CardAccountID);
            }
            return cards;
        }

        public bool IsCardExists(int CardID)
        {
            return cardRepository.IsCardExists(CardID);
        }
    }
}
