using Discount.Application.Commands;
using Discount.Application.Handlers.Queries;
using Discount.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handlers.Commands
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<GetDiscountQueryHandler> _logger;
        public DeleteDiscountCommandHandler(IDiscountRepository discountRepository, ILogger<GetDiscountQueryHandler> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;

        }
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var deleted =await _discountRepository.DeleteDiscount(request.ProductName);
            return deleted;
        }
    }
}
