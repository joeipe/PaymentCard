using PaymentCard.Contracts.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentCard.Contracts
{
    public class CreateTransactionRequest
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Description must not exceed 50 characters.")]
        public string Description { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        [ValidDate(ErrorMessage = "TransactionDate must be a valid date.")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "CreditLimit must be a positive number.")]
        [DataType(DataType.Currency)]
        [TwoDecimalPlaces(ErrorMessage = "CreditLimit must have at most 2 decimal places.")]
        public decimal Amount { get; set; }

        [Required]
        public int CardId { get; set; }
    }
}
