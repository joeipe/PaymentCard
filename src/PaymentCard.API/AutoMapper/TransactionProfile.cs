using AutoMapper;
using PaymentCard.Contracts.PurchaseTransactions;
using PaymentCard.Domain.PurchaseTransactions;

namespace PaymentCard.API.AutoMapper
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<PurchaseTransaction, TransactionBaseResponse>()
                .ForMember(dest => dest.OriginalUsdAmount, opt => opt.MapFrom(src => src.Amount));
            CreateMap<PurchaseTransaction, TransactionResponse>()
                .ForMember(dest => dest.OriginalUsdAmount, opt => opt.MapFrom(src => src.Amount));
            CreateMap<CreateTransactionRequest, PurchaseTransaction>();
        }
    }
}