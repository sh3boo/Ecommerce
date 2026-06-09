using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entites;
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
        }
    }
}
