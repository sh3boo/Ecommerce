using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Context
{
    public static class BrandContextSeed
    {
        public static async Task SeedDataAsync(IMongoCollection<ProductBrand> brandCollection)
        {
            bool existBrand = await brandCollection.Find(b => true).AnyAsync();
            if (existBrand)
            {
                return;
            }
            var filepath= Path.Combine("Data","SeedData", "brands.json");
            if (File.Exists(filepath))
            {
                var brandData = await File.ReadAllTextAsync(filepath);
                var brands = System.Text.Json.JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                if (brands != null && brands.Count > 0)
                {
                    await brandCollection.InsertManyAsync(brands);
                }
                else {
                    return;
                }
            }

        }
    }
}
