using Dapper;
using Discount.Core.Entites;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:Connection"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                       "SELECT * FROM Coupon WHERE LOWER(TRIM(ProductName)) = LOWER(TRIM(@ProductName))",
                       new
                       {
                           ProductName = productName
                       });
            if (coupon == null)
            {
                return new Coupon { Amount = 0,Description="No Coupns Is Allowed for this Product",ProductName="No Discount" };
            }
            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:Connection"));
            var affected = await connection.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                coupon);

            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:Connection"));
            var affected = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE LOWER(TRIM(ProductName)) = LOWER(TRIM(@ProductName))",
                new
                {
                    ProductName = productName
                });

            return affected > 0;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:Connection"));
            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                coupon);

            return affected > 0;
        }
    }
}
