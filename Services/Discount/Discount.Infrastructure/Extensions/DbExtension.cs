using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var Services = scope.ServiceProvider;
                var config = Services.GetRequiredService<IConfiguration>();
                var logger = Services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Discount DB Migration Started");
                    ApplyMigrations(config);
                    logger.LogInformation("Discount DB Migration completed");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Can't create db migration");
                }
            }
            return host;
        }
        private static void ApplyMigrations(IConfiguration config) 
        {
            var retry = 5;
            while (retry-- > 0)
            {
                try
                {
                    using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:Connection"));
                    connection.Open();
                    using var cmd = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    cmd.CommandText = (@"Create Table IF NOT EXISTS Coupon (Id Serial Primary key,
                                        ProductName VARCHAR(500) Not Null,
                                        Description Text,
                                        Amount INT)");
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount)
                                        SELECT 'Egypt Adidas Quick Force Indoor Badminton Shoes', 'Addidas Discount', 600
                                        WHERE NOT EXISTS (
                                            SELECT 1 FROM Coupon
                                            WHERE ProductName = 'Egypt Adidas Quick Force Indoor Badminton Shoes'
                                        );";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount)
                                        SELECT 'PowerFit 19 FH Rubber Spike Cricket Shoes', 'PowerFit Discount', 600
                                        WHERE NOT EXISTS (
                                            SELECT 1 FROM Coupon
                                            WHERE ProductName = 'PowerFit 19 FH Rubber Spike Cricket Shoes'
                                        );";
                    cmd.ExecuteNonQuery();
                    break;

                }
                catch (Exception ex)
                {
                    if (retry == 0)
                    {
                        throw;
                    }
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
