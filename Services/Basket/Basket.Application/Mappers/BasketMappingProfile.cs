using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entites;
using EventBus.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Mappers
{
    public class BasketMappingProfile : Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<ShoppingCart,ShoppingCartResponse>().ReverseMap();
            CreateMap<ShoppingCartItem, ShoppingCartIemResponse>().ReverseMap();
            CreateMap<BasketCheckout, BasketCheckoutEvent>()
                .ForMember(destination => destination.TotaPrice,
                    options => options.MapFrom(source => source.TotalPrice))
                .ForMember(destination => destination.State,
                    options => options.MapFrom(source => source.City))
                .ForMember(destination => destination.CardName,
                    options => options.MapFrom(source => source.CartName))
                .ForMember(destination => destination.CardNumber,
                    options => options.MapFrom(source => source.CartNumber));
        }
    }
}
