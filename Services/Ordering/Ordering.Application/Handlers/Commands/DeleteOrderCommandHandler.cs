using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.Commands
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        public readonly ILogger<DeleteOrderCommandHandler> _logger;
        public readonly IMapper _mapper;
        public readonly IOrderRepository _orderRepository;
        public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null)
            {
                _logger.LogError($"Order with id {request.Id} not found.");
                throw new OrderNotFoundException("Order", request.Id);
            }
            await _orderRepository.DeleteAsync(order);
            _logger.LogInformation($"Order with id {request.Id} deleted successfully.");
            return Unit.Value;

        }
    }
}
