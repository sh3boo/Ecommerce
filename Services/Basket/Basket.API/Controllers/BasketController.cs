using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Handlers.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entites;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }
        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShoppingCartResponse>> CreateBasket(
    [FromBody] CreateShoppingCartCommand command)
        {
            if (command == null)
                return BadRequest("The basket request is null.");

            if (string.IsNullOrWhiteSpace(command.UserName))
                return BadRequest("UserName is required.");

            var basket = await _mediator.Send(command);

            if (basket == null)
            {
                return Problem(
                    title: "Basket creation failed",
                    detail: "The MediatR handler returned null. Check the command handler, AutoMapper configuration, Redis connection, and repository save method.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }

            return Ok(basket);
        }
        [HttpDelete]
        [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
        {
            var command = new DeleteBasketByUserNameCommand(userName);
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("Checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
            {
                return BadRequest($"Basket for user '{basketCheckout.UserName}' was not found.");
            }
            var eventMsg = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMsg.TotaPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMsg);
            var deletedcmd = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await _mediator.Send(deletedcmd);
            return Accepted();
        }
    }
}
