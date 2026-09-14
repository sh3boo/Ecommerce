using Microsoft.Extensions.Logging;
using Ordering.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext,ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seeded database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        public static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    UserName = "Sh3boo",
                    FirstName = "Sh3boo",
                    LastName = "Sh3boo",
                    EmailAddress = "sh3boo@example.com",
                    AddressLine = "123 Main St",
                    Country = "USA",
                    State = "CA",
                    ZipCode = "12345",
                    Phone = "123-456-7890",
                    CardName = "Visa",
                    CardNumber = "4111111111111111",
                        Expiration = "12/25",
                        cvv = "123",
                        PaymentMethod = 1,
                        TotaPrice = 100.00m,
                        LastModifiedBy = "Sh3boo",
                        LastModifiedDate = DateTime.UtcNow


                }

            };
        }
    }
}
