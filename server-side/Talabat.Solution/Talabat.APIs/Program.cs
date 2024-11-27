
using Microsoft.EntityFrameworkCore;
using Talabat.Domain.Layer.IRepositories;
using Talabat.Domain.Layer.Specifications;
using Talabat.Repository.Layer;
using Talabat.Repository.Layer.Data;
using Talabat.Repository.Layer.Data.Seeders;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.APIs.Extensions;
using StackExchange.Redis;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.AddSwaggerService(); // extension method to cleanup services


            builder.Services.AddDbContext<StoreContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddSingleton<IConnectionMultiplexer>(S => {
                try
                {
                    string Redis = builder.Configuration.GetConnectionString("Redis")!;
                    ConfigurationOptions ConnectionString = ConfigurationOptions.Parse(Redis, true);
                    return ConnectionMultiplexer.Connect(ConnectionString);
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not connect to Redis", ex);
                }
            });

            builder.AddApplicationService();  // extension method to cleanup services


            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>(); // add custom middleware

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation(); // extension method to cleanup services
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            // Use Cors

            // Use Authentication

            // Use Authorization();

            app.MapControllers();

            // Seed the database after everything is set up
            using (var scope = app.Services.CreateScope())
            {
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
                    await context.Database.MigrateAsync();
                    await StoreContextSeeding.SeedingDataAsync(context, loggerFactory);
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred during migration or seeding.");
                }
            }

            app.Run();
        }
    }
}
