using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Contracts
{
    public class TransactionResponse
    {
        public int Id { get; set; }
        public string Description { get; set; } = default!;
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int CardId { get; set; }
    }
}
