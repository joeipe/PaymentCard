namespace PaymentCard.Contracts
{
    public class CardResponse
    {
        public int Id { get; set; }
        public string CardNumber { get; set; } = default!;
        public decimal CreditLimit { get; set; }
    }
}