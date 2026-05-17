using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Context
{
    public class CatalogContext : ICatalogContext
    {
        IMongoCollection<Product> Products
        { get; set; }

        IMongoCollection<ProductBrand> ProductBrands
            { get; set; }

        IMongoCollection<ProductType> ProductTypes
            { get; set; }

        IMongoCollection<Product> ICatalogContext.Products => Products;

        IMongoCollection<ProductBrand> ICatalogContext.ProductBrands => ProductBrands;

        IMongoCollection<ProductType> ICatalogContext.ProductTypes => ProductTypes;

        public CatalogContext(IConfiguration configuration)
        {
            var Client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var database = Client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            Products = database.GetCollection<Product>(configuration["DatabaseSettings:ProductsCollection"]);
            ProductBrands = database.GetCollection<ProductBrand>(configuration["DatabaseSettings:BrandsCollection"]);
            ProductTypes = database.GetCollection<ProductType>(configuration["DatabaseSettings:TypesCollection"]);
            _= CatalofContextSeed.SeedDataAsync(Products);
            _= BrandContextSeed.SeedDataAsync(ProductBrands);
            _= TypeContextSeed.SeedDataAsync(ProductTypes);
        }
    }
}
