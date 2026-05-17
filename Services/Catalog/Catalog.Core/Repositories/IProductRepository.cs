using Catalog.Core.Entities;
using Catalog.Core.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository  
    {
        Task<Pagination<Product>> GetAllProductsAsync(CatalogSpecParam catalogSpecParam);
        Task<IEnumerable<Product>> GetAllProductsByName(string Name);
        Task<IEnumerable<Product>> GetAllProductsByBrand(string Name);
        Task<Product> GetProductById(string id);
        Task<Product> GetProductByName(string name);
        Task<Product> GetProductById(int id);
        Task<Product> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string Id);
    }
}
