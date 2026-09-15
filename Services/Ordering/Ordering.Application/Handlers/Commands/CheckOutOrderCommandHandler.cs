using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.Commands
{
    public class CheckOutOrderCommandHandler : IRequestHandler<CheckOutOrderCommand, int>
    {
        public readonly ILogger<CheckOutOrderCommandHandler> _logger;
        public readonly IMapper _mapper;
        public readonly IOrderRepository _orderRepository;
        public CheckOutOrderCommandHandler(ILogger<CheckOutOrderCommandHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(CheckOutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Core.Entites.Order>(request);
            var generatedOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {generatedOrder.Id} is successfully created.");
            return generatedOrder.Id;
        }
    }
}
