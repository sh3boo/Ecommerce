using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, List<BrandResponseDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        public GetAllBrandsQueryHandler(
            IMapper mapper,
            IBrandRepository brandRepository
            )
        {

           _brandRepository = brandRepository;
           _mapper = mapper;

        }
        public async Task<List<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brandsList = await _brandRepository.GetAllBrands();
            var brandsResponseList = _mapper.Map<IList<ProductBrand>,IList<BrandResponseDto>>(brandsList.ToList());
            return brandsResponseList.ToList();
        }
    }
}
