namespace PaymentCard.Application.Interfaces.Data
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}