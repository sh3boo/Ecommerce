using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{

    public class CatalogController : BaseApiController
    {
        private readonly IMediator _mediator;
        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet]
        [Route("[action]/{id}", Name ="GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponseDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductResponseDto>> GetProductById([FromRoute] string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
            //return Ok("This is Catalog API");
        }


        [HttpGet]
        [Route("[action]/{ProductName}", Name = "GetProductsByProductName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductResponseDto>))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductResponseDto>> GetProductsByProductName(string ProductName)
        {
            var query = new GetProductsByNameQuery(ProductName);
            var result = await _mediator.Send(query);
            return Ok(result);
            //return Ok("This is Catalog API");
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductResponseDto>))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductResponseDto>> GetAllProducts([FromQuery] CatalogSpecParam Spec )
        {
            var query = new GetAllProductsQuery(Spec);
            var result = await _mediator.Send(query);
            return Ok(result);
            //return Ok("This is Catalog API");
        }

        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<BrandResponseDto>))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<BrandResponseDto>> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
            //return Ok("This is Catalog API");
        }


        [HttpGet]
        [Route("GetAllTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<TypeResponseDto>))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<TypeResponseDto>> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
            //return Ok("This is Catalog API");
        }


        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponseDto))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] CreateProductCommand productCommand)
        {
            var result = await _mediator.Send<ProductResponseDto>(productCommand);
            return Ok(result);
        }


        [HttpPost]
        [Route("UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct([FromBody] UpdateProductCommand productCommand)
        {
            var result = await _mediator.Send<bool>(productCommand);
            return Ok(result);
        }


        [HttpPost]
        [Route("DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductResponseDto>> DeleteProduct(string id)
        {
            var command = new DeleteProductCommand(id);
            var result = await _mediator.Send<bool>(command);
            return Ok(result);
        }
    }
}
 