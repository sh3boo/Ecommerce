
using Discount.API.Services;
using Discount.Application.Commands;
using Discount.Application.Mapper;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;

namespace Discount.API
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

            builder.Services.AddAutoMapper(cfg =>
             cfg.AddMaps(typeof(DiscountProfile).Assembly));

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateDiscountCommand).Assembly);
            });

            builder.Services.AddScoped<IDiscountRepository, IDiscountRepository>();
            builder.Services.AddGrpc();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

                //app.MapOpenApi();
                app.UseDeveloperExceptionPage();
            }
            app.MigrateDatabase<Program>();
            app.UseRouting();
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapGrpcService<DiscountService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with GRPC Endpoints must be made throw a grpc client");
                });
            } );
            //app.UseAuthorization();


            //app.MapControllers();

            app.Run();
        }
    }
}
