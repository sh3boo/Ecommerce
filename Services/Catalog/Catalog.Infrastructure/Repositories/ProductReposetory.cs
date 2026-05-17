using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductReposetory : IBrandRepository, ITypeRepository, IProductRepository
    {
        public ICatalogContext _context { get; set; }
        public ProductReposetory(ICatalogContext context)
        {
            _context = context;
        }
        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Pagination<Product>> GetAllProductsAsync(CatalogSpecParam catalogSpecParam)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(catalogSpecParam.search))
            {
                filter = filter & builder.Where(p => p.Name.ToLower().Contains(catalogSpecParam.search.ToLower()));
            }
            if (!string.IsNullOrEmpty(catalogSpecParam.BrabdId))
            {
                var BrandFilter = builder.Eq(p => p.Brand.Id, catalogSpecParam.BrabdId);
                filter &= BrandFilter;
            }
            if (!string.IsNullOrEmpty(catalogSpecParam.TypeId))
            {
                var TypeFilter = builder.Eq(p => p.Type.Id, catalogSpecParam.TypeId);
                filter &= TypeFilter;
            }
            var totalItems = await _context.Products.CountDocumentsAsync(filter);
            var data = await DataFilter(catalogSpecParam, filter);

            return new Pagination<Product>(catalogSpecParam.PageIndex, catalogSpecParam.PageSize, (int)totalItems, data);
        }

        public async Task<IEnumerable<Product>> GetAllProductsByBrand(string Name)
        {
            return await _context.Products.Find(p => p.Brand.Name == Name).ToListAsync();
        }


        public async Task<IEnumerable<Product>> GetAllProductsByName(string Name)
        {
            return await _context.Products.Find(p => p.Name == Name).ToListAsync();
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string Id)
        {
            var deleted = await _context.Products.DeleteOneAsync(p => p.Id == Id);
            return deleted.IsAcknowledged && deleted.DeletedCount > 0;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context.ProductBrands.Find(p => true).ToListAsync();
        }



        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _context.ProductTypes.Find(p => true).ToListAsync();
        }



        public Task<Product> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetProductByName(string name)
        {
            return await _context.Products.Find(p => p.Name == name).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updated = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updated.IsAcknowledged && updated.ModifiedCount > 0;
        }
        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParam catalogSpecParam, FilterDefinition<Product> filter)
        {
            var sortDfn = Builders<Product>.Sort.Descending(p => p.Name);
            if (!string.IsNullOrEmpty(catalogSpecParam.Sort))
            {
                switch (catalogSpecParam.Sort.ToLower())
                {
                    case "priceasc":
                        sortDfn = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "pricedesc":
                        sortDfn = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortDfn = Builders<Product>.Sort.Descending(p => p.Name);
                        break;
                }
            }
            return await _context
                .Products
                .Find(filter)
                .Sort(sortDfn)
                .Skip(catalogSpecParam.PageSize * (catalogSpecParam.PageIndex - 1))
                .Limit(catalogSpecParam.PageSize)
                .ToListAsync();
        }
    }
}
