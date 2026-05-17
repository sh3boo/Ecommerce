using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Context
{
    public static class CatalofContextSeed
    {
        public static async Task SeedDataAsync(IMongoCollection<Product> ProductCollection)
        {
            bool existProduct = await ProductCollection.Find(b => true).AnyAsync();
            if (existProduct)
            {
                return;
            }
            var filepath = Path.Combine("Data", "SeedData", "products.json");
            if (File.Exists(filepath))
            {
                var ProductData = await File.ReadAllTextAsync(filepath);
                var Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products != null && Products.Count > 0)
                {
                    await ProductCollection.InsertManyAsync(Products);
                }
                else
                {
                    return;
                }
            }

        }
    }
}
