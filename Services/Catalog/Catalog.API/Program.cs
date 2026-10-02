
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Context;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog(Logging.ConfigreLogger);

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://identityserver:9011";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://id-local.eshopping.com:44344",
                        ValidateAudience = true,
                        ValidAudience= "Catalog",
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey=true,
                        ClockSkew=TimeSpan.Zero
                    };
                    // add this to docker to host communication
                    options.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context=>
                        {
                            Console.WriteLine("=====  Authintcation failed");
                            Console.WriteLine($"Exception {context.Exception.Message}");
                            Console.WriteLine($"Authurity {options.Authority}");
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CanRead", policy => policy.RequireClaim("scope", "Catalogapi.read"));
            });

            builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
            //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            //    Assembly.GetExecutingAssembly(),
            //    Assembly.GetAssembly(typeof(ProductMappingProfile))

            //    ));
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ProductMappingProfile).Assembly);
            });


            builder.Services.AddScoped<ICatalogContext, CatalogContext>();
            builder.Services.AddScoped<IProductRepository, ProductReposetory>();
            builder.Services.AddScoped<ITypeRepository, ProductReposetory>();
            builder.Services.AddScoped<IBrandRepository, ProductReposetory>();

            var userPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            builder.Services.AddControllers(confg =>
            {
                confg.Filters.Add(new AuthorizeFilter(userPolicy));
            });

            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options=>
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

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter the JWT access token only. Swagger adds the Bearer prefix automatically.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // BEFORE UseSwagger / routing
                app.Use((ctx, next) =>
                {
                    if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var p) && !string.IsNullOrEmpty(p))
                        ctx.Request.PathBase = p.ToString();   // e.g., "/catalog"
                    return next();
                });

                app.UseSwagger(c =>
                {
                    // Make the OpenAPI "servers" base path match the prefix so Try it out uses /catalog/...
                    c.PreSerializeFilters.Add((doc, req) =>
                    {
                        var prefix = req.Headers["X-Forwarded-Prefix"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(prefix))
                            doc.Servers = new List<OpenApiServer>
            { new() { Url = prefix } };
                    });
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Catalog.API v1"); // relative path (no leading '/')
                    c.RoutePrefix = "swagger";
                });

            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
