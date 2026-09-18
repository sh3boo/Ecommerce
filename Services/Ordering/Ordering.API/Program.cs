
using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

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
                    Title = "Catalog.API",
                    Version = "v1",
                    Description = "Catalog Microservice API",
                    Contact = new Microsoft.OpenApi.OpenApiContact
                    {
                        Name = "Ahemd Shaban",
                        Email = "ahmedshanan2021@gmail.com"
                    }
                }
                    );
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddInfraService(builder.Configuration);
            builder.Services.AddScoped<BasketOrderingConsumer>();

            //conf related to rabbit mq
            builder.Services.AddMassTransit(config =>
            {
                // mark consumer
                config.AddConsumer<BasketOrderingConsumer>();
                config.UsingRabbitMq((ct, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<BasketOrderingConsumer>(ct);
                    });
                });

            });
            builder.Services.AddMassTransitHostedService();


            builder.Services.AddControllers();
            var app = builder.Build();
            app.MigrateDatabase<OrderContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<OrderContextSeed>>();
                OrderContextSeed.SeedAsync(context, logger).Wait();
            });

            // Configure the HTTP request pipeline.
             if (app.Environment.IsDevelopment())
            {
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
