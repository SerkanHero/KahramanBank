using SerkanK.Database;
using SerkanK.Models;

namespace SerkanK.Repository
{

    public interface ICardRepository
    {
        public bool IsCardExists(int ID);
        public Card? GetCard(string CardNumber);
        public Card? GetCard(int IDorCardHolderID);
        public List<Card> GetCardWithHolder(int CardHolderID);
        public bool AddCard(int UserID, int AccountID, int CardType);
        public bool dbCheck();
    }

    public class CardRepository : ICardRepository
    {
        SystemDBContext context;

        public CardRepository(SystemDBContext _context)
        {
            context = _context;
            context.StartUp();
        }

        public string CardNumberGenerate(int UserID)
        {
            string CardPrefix = "1";
            string BankCode = "11";

            Random r = new Random();
            int eightDigit = r.Next(10000000, 100000000);
            return CardPrefix + BankCode + UserID.ToString().PadLeft(5, '0') + eightDigit.ToString().PadLeft(8, '1');
        }

        public int CVVGenerate()
        {
            Random r = new Random();
            return r.Next(100, 1000);
        }

        public bool dbCheck() => context.StartUpCheck;
        public bool IsCardExists(int ID) => context.Cards.Any(u => u.CardID == ID);
        public Card? GetCard(int ID) => context.Cards.FirstOrDefault(u => u.CardID == ID);
        public List<Card> GetCardWithHolder(int CardHolderID) => context.Cards.Where(u => u.CardHolderID == CardHolderID).ToList();
        public Card? GetCard(string CardNumber) => context.Cards.FirstOrDefault(u => u.CardNumber == CardNumber);
        public bool AddCard(int UserID, int AccountID, int CardType = 0)
        {
            Card C = new Card();
            C.CardHolderID = UserID;
            C.CVV = CVVGenerate();
            C.CardNumber = CardNumberGenerate(UserID);
            C.ExpirationDate = DateTime.Now.AddYears(2);
            C.IsContactlessPaymentEnabled = true;
            C.IsOnlineShoppingEnabled = true;

            context.Cards.Add(C);
            context.SaveChanges();
            return true;
        }
    }
}
