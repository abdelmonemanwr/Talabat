using Microsoft.AspNetCore.Identity;
using Talabat.Domain.Layer.Entities.Identity;
using Talabat.Repository.Layer.Identity;

namespace Talabat.APIs.Extensions
{
    public static class AddIdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    options.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            Services.AddAuthentication();

            return Services;
        }
    }
}
