
using Basket.Application.Handlers.Commands;
using Basket.Application.Mappers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(BasketMappingProfile).Assembly));
            //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            //    Assembly.GetExecutingAssembly(),
            //    Assembly.GetAssembly(typeof(ProductMappingProfile))

            //    ));
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateShoppingCartCommand).Assembly);
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
                {
                    Title = "Basket.API",
                    Version = "v1",
                    Description = "Basket Microservice API",
                    Contact = new Microsoft.OpenApi.OpenApiContact
                    {
                        Name = "Ahemd Shaban",
                        Email = "ahmedshanan2021@gmail.com"
                    }
                }
                    );
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionStrings");
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
