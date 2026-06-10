using AutoMapper;
using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers.Commands
{
    public class DeleteBasketByUserNameCommandHandler : IRequestHandler<DeleteBasketByUserNameCommand, Unit>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public DeleteBasketByUserNameCommandHandler(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteBasketByUserNameCommand request, CancellationToken cancellationToken)
        {
            await _basketRepository.DeleteBasket(request.UserName);
            return  Unit.Value;
        }
    }
}
