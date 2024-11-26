using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Repository.Layer.Data.Seeders
{
    public class StoreContextSeeding
    {
        private static readonly List<(IDataSeeder seeder, string fileName)> Seeders = 
            new List<(IDataSeeder seeder, string fileName)>() { 
                (new ProductTypeSeeder(), "types.json"), 
                (new ProductBrandSeeder(), "brands.json"), 
                (new ProductSeeder(), "products.json") 
            };

        public static async Task SeedingDataAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<StoreContextSeeding>();
            try
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var solutionRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.FullName;

                if (solutionRoot == null)
                {
                    throw new InvalidOperationException("Could not find solution root directory.");
                }

                var dataSeedingPath = Path.GetFullPath(Path.Combine(solutionRoot, "..\\..", "Talabat.Repository", "Data", "DataSeeding"));


                bool state = true;
                foreach (var (seeder, fileName) in Seeders)
                {
                    var seedInfo = await context.SeedInfo.FirstOrDefaultAsync(s => s.SeedName == fileName);

                    if (seedInfo == null || !seedInfo.IsSeeded)
                    {
                        var filePath = Path.Combine(dataSeedingPath, fileName);
                        logger.LogInformation($"Seeding file: {filePath}");
                        state = await seeder.SeedDataAsync(context, filePath);

                        // Mark the seed as completed
                        if (seedInfo == null)
                        {
                            context.SeedInfo.Add(new SeedInfo
                            {
                                SeedName = fileName,
                                IsSeeded = state,
                                LastSeeded = DateTime.UtcNow
                            });
                        }
                        else
                        {
                            seedInfo.IsSeeded = true;
                            seedInfo.LastSeeded = DateTime.UtcNow;
                        }

                        await context.SaveChangesAsync();
                    }
                }

                if (state == true)
                {
                    logger.LogInformation("Seeding process completed.");
                }
                else
                {
                    logger.LogError("Seeding process Failed.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Seeding process failed: {ex.Message}");
            }
        }
    }
}