using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Contracts
{
    public class TransactionBaseResponse
    {
        public int Id { get; set; }
        public string Description { get; set; } = default!;
        public DateTime TransactionDate { get; set; }
        public decimal OriginalUsdAmount { get; set; }
        public int CardId { get; set; }
    }
}
