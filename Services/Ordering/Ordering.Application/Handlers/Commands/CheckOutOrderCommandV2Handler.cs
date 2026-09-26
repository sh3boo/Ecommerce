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
    internal class CheckOutOrderCommandV2Handler : IRequestHandler<CheckOutOrderCommandV2, int>
    {
        public readonly ILogger<CheckOutOrderCommandV2Handler> _logger;
        public readonly IMapper _mapper;
        public readonly IOrderRepository _orderRepository;
        public CheckOutOrderCommandV2Handler(ILogger<CheckOutOrderCommandV2Handler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(CheckOutOrderCommandV2 request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Core.Entites.Order>(request);
            var generatedOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {generatedOrder.Id} is successfully created with v2");
            return generatedOrder.Id;
        }
    }
}
