using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;


namespace Ordering.Infrastructure.Extensions
{
    public static class InfraService
    {
        public static IServiceCollection AddInfraService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("OrderingConnectionString"),
                SqlServerOptions => SqlServerOptions.EnableRetryOnFailure()

                )
            );
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}