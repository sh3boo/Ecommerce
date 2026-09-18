using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger <BasketOrderingConsumer> _logger;
        public BasketOrderingConsumer( IMapper mapper,IMediator mediator , ILogger<BasketOrderingConsumer> logger)
        {
            _mapper = mapper;
            
            _mediator = mediator;
            _logger = logger;
            
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            using var scope = _logger.BeginScope("Consuming Basket Checout event for {CorrelationId}", context.Message.CorrelationId);
            var cmd = _mapper.Map<CheckOutOrderCommand>(context.Message);
            var result = await _mediator.Send(cmd);
            _logger.LogInformation("Basket Checout Event Completed");
            
        }
    }
}
