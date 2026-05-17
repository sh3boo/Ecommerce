using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetAllProductsQuery : IRequest<Pagination<ProductResponseDto>>
    {
        public CatalogSpecParam Spec { get; set; }

        public GetAllProductsQuery(CatalogSpecParam Spec)
        {
            this.Spec = Spec;

        }


    }
}
