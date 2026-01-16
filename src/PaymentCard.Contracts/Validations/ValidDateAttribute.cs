using System.ComponentModel.DataAnnotations;

namespace PaymentCard.Contracts.Validations
{
    public sealed class ValidDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;

            if (value is DateTime dt)
            {
                // Treat default/MinValue as invalid input
                return dt == default(DateTime) || dt == DateTime.MinValue
                    ? new ValidationResult(ErrorMessage ?? "Date is not valid.")
                    : ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Invalid data type for date validation.");
        }
    }
}