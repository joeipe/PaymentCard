using PaymentCard.Domain.Cards;
using PaymentCard.Domain.Common;

namespace PaymentCard.Domain.PurchaseTransactions
{
    public class PurchaseTransaction : Entity
    {
        public string Description { get; set; } = default!;
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; } = default!;
    }
}