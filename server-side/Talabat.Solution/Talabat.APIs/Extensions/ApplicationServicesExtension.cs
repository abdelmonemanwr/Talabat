using Talabat.APIs.Helpers;
using Talabat.Domain.Layer.IRepositories;
using Talabat.Repository.Layer;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationService(this WebApplicationBuilder builder) 
        {
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //builder.Services.AddAutoMapper(Profile => Profile.AddProfile(new MappingProfiles()));
            builder.Services.AddAutoMapper(typeof(MappingProfiles)); // Simple Way

            #region Logger
            // Suppress SQL command logs
            builder.Logging
                .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning)
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information); // Show other info level logs
            #endregion

            return builder.Services;
        }
    }
}
