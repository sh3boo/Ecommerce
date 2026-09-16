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
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        public readonly ILogger<UpdateOrderCommandHandler> _logger;
        public readonly IMapper _mapper;
        public readonly IOrderRepository _orderRepository;
        public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null)
            {
                _logger.LogError($"Order with id {request.Id} not found.");
                throw new OrderNotFoundException("Order", request.Id);
            }
            await _orderRepository.UpdateAsync(order);
            _logger.LogInformation($"Order with id {request.Id} Updated successfully.");
            return Unit.Value;
        }
    }
}
