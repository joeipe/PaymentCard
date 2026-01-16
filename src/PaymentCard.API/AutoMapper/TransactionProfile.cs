using AutoMapper;
using PaymentCard.Contracts;
using PaymentCard.Domain;

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