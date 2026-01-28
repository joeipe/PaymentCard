using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;

namespace PaymentCard.Domain
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