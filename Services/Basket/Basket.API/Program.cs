using Basket.Application.GrpcServices;
using Basket.Application.Handlers.Commands;
using Basket.Application.Mappers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog(Logging.ConfigreLogger);


            builder.Services.AddControllers();
             
            builder.Services.AddAutoMapper(cfg =>
                cfg.AddMaps(typeof(BasketMappingProfile).Assembly));

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateShoppingCartCommand).Assembly);
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<DiscountGrpcService>();
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
                cfg=>cfg.Address=new Uri(builder.Configuration["GrpcSettings:DiscountUrl"])
                );

            //conf related to rabbit mq
            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ct, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                });

            });
            builder.Services.AddMassTransitHostedService();

            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Basket.API",
                    Version = "v1",
                    Description = "Basket Microservice API",
                    Contact = new OpenApiContact
                    {
                        Name = "Ahmed Shaban",
                        Email = "ahmedshaban2021@gmail.com"
                    }
                });
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1");
                options.RoutePrefix = "swagger";
            });

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}