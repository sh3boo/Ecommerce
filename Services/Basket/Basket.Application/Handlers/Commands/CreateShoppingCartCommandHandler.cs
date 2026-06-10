using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers.Commands
{
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.UpdateBasket(new Core.Entites.ShoppingCart
            {
                UserName = request.UserName,
                Items = request.Items
            });

            var ShoppingCartResponse = _mapper.Map<ShoppingCartResponse>(shoppingCart);
            return ShoppingCartResponse;
        }
    }
}
