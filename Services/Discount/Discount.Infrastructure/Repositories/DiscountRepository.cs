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

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            // Ensure NpgsqlConnection is properly referenced
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:Connection"));
            var affected = connection.ExecuteAsync
                ("Insert into coupon (ProductName , Description , Amount) Values (@ProductName , @Desctiption , @Amount",
                new
                {
                    Amount = coupon.Amount,
                    Description = coupon.Description,
                    ProductName = coupon.ProductName
                });
            if (affected == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            // Ensure NpgsqlConnection is properly referenced
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:Connection"));
            var affected = connection.ExecuteAsync
                (" delete from coupon where ProductName = @ProductName ",
                new
                {
                    
                    ProductName = productName

                });
            if (affected == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            // Ensure NpgsqlConnection is properly referenced
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:Connection"));
            var affected = connection.ExecuteAsync
                (" update coupon set ProductName = @ProductName , Descreption=@Descreption,Amount=@Amoun where Id =@Id",
                new
                {
                    Amount = coupon.Amount,
                    Description = coupon.Description,
                    ProductName = coupon.ProductName,
                    Id=coupon.Id
                });
            if (affected == null)
            {
                return false;
            }
            return true;

         }
    }
}
