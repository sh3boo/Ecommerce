using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.Queries
{
    public class GetPtoductsByBrandQueryHandler : IRequestHandler<GetPtoductsByBrandQuery, IList<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetPtoductsByBrandQueryHandler(
             IProductRepository productRepository,
              IMapper mapper
            )
        {
            _productRepository = productRepository;
            _mapper = mapper;

        }
        public async Task<IList<ProductResponseDto>> Handle(GetPtoductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var Products = await _productRepository.GetAllProductsByBrand(request.BrandName);
            var productResponseList = _mapper.Map<IList<ProductResponseDto>>(Products);
            return  productResponseList;
        }
    }
}
