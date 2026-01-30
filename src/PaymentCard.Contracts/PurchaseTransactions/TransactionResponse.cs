namespace PaymentCard.Contracts.PurchaseTransactions
{
    public class TransactionResponse : TransactionBaseResponse
    {
        public decimal? ExchangeRateUsed { get; set; }
        public decimal? ConvertedAmount { get; set; }
        public string? TargetCurrency { get; set; }
        public string? ErrorMessage { get; set; }
    }
}