using AutoMapper;
using Discount.Application.Commands;
using Discount.Application.Handlers.Queries;
using Discount.Core.Entites;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handlers.Commands
{
    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<GetDiscountQueryHandler> _logger;
        private readonly IMapper _mapper;
        public CreateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            
        }

        public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<Coupon>(request);
            await _discountRepository.CreateDiscount(coupon);
            var coupnModel=_mapper.Map<CouponModel>(coupon);
            return coupnModel;
        }
    }
}
