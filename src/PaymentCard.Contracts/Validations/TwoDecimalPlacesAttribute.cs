using System.ComponentModel.DataAnnotations;

namespace PaymentCard.Contracts.Validations
{
    public sealed class TwoDecimalPlacesAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;

            if (value is decimal d)
            {
                var bits = decimal.GetBits(d);
                var scale = (bits[3] >> 16) & 0xFF;
                return scale <= 2
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorMessage ?? "Value must have at most 2 decimal places.");
            }

            return new ValidationResult(ErrorMessage ?? "Invalid data type for decimal validation.");
        }
    }
}