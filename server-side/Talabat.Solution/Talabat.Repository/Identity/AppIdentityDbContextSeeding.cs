using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities.Identity;
using Talabat.Repository.Layer.Data.Seeders;

namespace Talabat.Repository.Layer.Identity
{
    public class AppIdentityDbContextSeeding
    {
        public static async Task SeedingDataAsync(UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    DisplayName = "Men3m",
                    UserName = "Abdalmonem",
                    Email = "abdelmonemanwr7777@gmail.com",
                    PhoneNumber = "01026050918",
                    AccessFailedCount = 0,
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    TwoFactorEnabled = false,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Address = new Address
                    {
                        Building = 2,
                        Country = "Egypt",
                        State = "Qalyubia",
                        City = "Al Qanatir Al Khayriah",
                        Street = "Salah Khaled",
                        FirstName = "Ahmed Salah",
                        ZipCode = "13789",
                    }
                };

                string password = "Men3m_44";

                var result = await userManager.CreateAsync(user, password);

                var logger = loggerFactory.CreateLogger<AppIdentityDbContextSeeding>();
                if (result.Succeeded)
                {
                    logger.LogInformation("Identity Seeding Process Completed Successfully\n");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        logger.LogError($"Error: {error.Description}\n");
                    }
                }

            }
        }
    }
}
