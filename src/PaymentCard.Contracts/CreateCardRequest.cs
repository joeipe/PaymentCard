using PaymentCard.Contracts.Validations;
using System.ComponentModel.DataAnnotations;

namespace PaymentCard.Contracts
{
    public class CreateCardRequest
    {
        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card Number must be a 16-digit numeric string.")]
        public string CardNumber { get; set; } = default!;

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "CreditLimit must be a positive number.")]
        [DataType(DataType.Currency)]
        [TwoDecimalPlaces(ErrorMessage = "CreditLimit must have at most 2 decimal places.")]
        public decimal CreditLimit { get; set; }
    }
}