using AutoMapper;
using PaymentCard.Contracts;
using PaymentCard.Domain;

namespace PaymentCard.API.AutoMapper
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardResponse>();
            CreateMap<CreateCardRequest, Card>();
            CreateMap<UpdateCardRequest, Card>();
        }
    }
}