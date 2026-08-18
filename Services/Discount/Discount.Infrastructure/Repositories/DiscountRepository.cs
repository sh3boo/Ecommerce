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
            // Ensure NpgsqlConnection is properly referenced
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:Connection"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                       "SELECT * FROM Coupon WHERE ProductName = @ProductName",
                       new
                       {
                           ProductName = productName
                       });
            if (coupon == null)
            {
                return new Coupon { Amount = 0,Description="No Coupns Is Allowed for this Product",ProductName="No Discount" };
            }
            return coupon;
            // Additional implementation can be added here
        }

        public Task<bool> CreateDiscount(Coupon coupon)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDiscount(string productName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDiscount(Coupon coupon)
        {
            throw new NotImplementedException();
        }
    }
}
