using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Context
{
    public static class TypeContextSeed
    {
        public static async Task SeedDataAsync(IMongoCollection<ProductType> TypeCollection)
        {
            bool existType = await TypeCollection.Find(b => true).AnyAsync();
            if (existType)
            {
                return;
            }
            var filepath = Path.Combine("Data", "SeedData", "types.json");
            if (File.Exists(filepath))
            {
                var TypeData = await File.ReadAllTextAsync(filepath);
                var Types = System.Text.Json.JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                if (Types != null && Types.Count > 0)
                {
                    await TypeCollection.InsertManyAsync(Types);
                }
                else
                {
                    return;
                }
            }

        }
    }
}
