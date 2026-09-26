using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<BasketOrderingConsumerV2> _logger;
        public BasketOrderingConsumerV2(IMapper mapper, IMediator mediator, ILogger<BasketOrderingConsumerV2> logger)
        {
            _mapper = mapper;

            _mediator = mediator;
            _logger = logger;

        }
        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            using var scope = _logger.BeginScope("Consuming Basket Checout event for {CorrelationId} with v2", context.Message.CorrelationId);
            var cmd = _mapper.Map<CheckOutOrderCommandV2>(context.Message);
            var result = await _mediator.Send(cmd);
            _logger.LogInformation("Basket Checout Event Completed V2 ");

        }
    }
}
