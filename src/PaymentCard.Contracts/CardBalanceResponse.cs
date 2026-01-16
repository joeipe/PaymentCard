using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCard.Contracts
{
    public class CardBalanceResponse : CardResponse
    {
        public decimal AvailableBalanceInUsd { get; set; }
        public decimal AvailableBalanceConverted { get; set; }
        public string? TargetCurrency { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
