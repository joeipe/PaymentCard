using AutoMapper;
using PaymentCard.Contracts;
using PaymentCard.Domain;

namespace PaymentCard.API.AutoMapper
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<PurchaseTransaction, TransactionResponse>();
            CreateMap<CreateTransactionRequest, PurchaseTransaction>();
        }
    }
}