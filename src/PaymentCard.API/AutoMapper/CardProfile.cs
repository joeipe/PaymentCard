using AutoMapper;
using PaymentCard.Contracts.Cards;
using PaymentCard.Domain.Cards;

namespace PaymentCard.API.AutoMapper
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardResponse>();
            CreateMap<Card, CardBalanceResponse>();
            CreateMap<CreateCardRequest, Card>();
            CreateMap<UpdateCardRequest, Card>();
        }
    }
}