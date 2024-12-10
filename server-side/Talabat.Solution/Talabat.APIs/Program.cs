
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
using Talabat.Repository.Layer.Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Domain.Layer.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;
using Talabat.Domain.Layer.IServices;

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

            builder.Services.AddSwaggerService(); // extension method to cleanup services

            builder.Services.AddApplicationService();  // extension method to cleanup services

            #region Cors
            string angularOrigin = builder.Configuration["Audience"]!;
            builder.Services.AddCors(
                options => {
                    options.AddPolicy("CorsPolicies", builder => {
                        builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(angularOrigin);
                    });
                }
            );
            #endregion

            #region Session Configuration
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            #endregion

            #region Logger
            // Suppress SQL command logs
            builder.Logging
                .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning)
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information); // Show other info level logs
            #endregion

            #region Database Contexts Configuration
            builder.Services.AddDbContext<StoreContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"), 
                    sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)
                )
            );

            builder.Services.AddDbContext<AppIdentityDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("IdentityConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)
                )
            );

            builder.Services.AddIdentityServices(builder.Configuration); // extension method to cleanup services
            #endregion

            #region Redis
            string RedisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
            builder.Services.AddSingleton<IConnectionMultiplexer>(S => {
                try
                {
                    ConfigurationOptions ConnectionString = ConfigurationOptions.Parse(RedisConnectionString, true);
                    return ConnectionMultiplexer.Connect(ConnectionString);
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not connect to Redis", ex);
                }
            });

            // Configure distributed caching with Redis
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = RedisConnectionString;
                options.InstanceName = "TalabatInstance";
            });
            #endregion

            #region Health Checks
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<StoreContext>()
                .AddDbContextCheck<AppIdentityDbContext>()
                .AddRedis(RedisConnectionString);
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation(); // extension method to cleanup services
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseCors("CorsPolicies");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>(); // add custom middleware

            app.MapHealthChecks("/health");

            app.MapControllers();

            #region Seed the database after everything is set up
            using (var scope = app.Services.CreateScope())
            {
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
                    await context.Database.MigrateAsync();

                    await StoreContextSeeding.SeedingDataAsync(context, loggerFactory);

                    var identityContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.MigrateAsync();

                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    await AppIdentityDbContextSeeding.SeedingDataAsync(userManager, loggerFactory);
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred during migration or seeding.\n");
                }
            }
            #endregion
            
            app.Run();
        }
    }
}
