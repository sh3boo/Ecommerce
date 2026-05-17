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
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, List<TypeResponseDto>>
    {
         private readonly ITypeRepository _typeRepository;
        private readonly IMapper _mapper;
        public GetAllTypesQueryHandler(
            IMapper mapper,
            ITypeRepository TypeRepository
            )
        {

            _typeRepository = TypeRepository;
           _mapper = mapper;

        }
        public async Task<List<TypeResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var TypesList = await _typeRepository.GetAllTypes();
            var TypesResponseList = _mapper.Map<IList<ProductType>,IList<TypeResponseDto>>(TypesList.ToList());
            return TypesResponseList.ToList();
        }
    }
}
