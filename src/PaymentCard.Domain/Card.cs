using SharedKernel;

namespace PaymentCard.Domain
{
    public class Card : Entity
    {
        public string CardNumber { get; set; } = default!;
        public decimal CreditLimit { get; set; }

        public List<PurchaseTransaction> Transactions { get; set; } = default!;
    }
}