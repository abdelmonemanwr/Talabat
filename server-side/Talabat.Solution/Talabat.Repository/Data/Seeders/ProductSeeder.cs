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
    public class ProductSeeder : IDataSeeder
    {
        public async Task<bool> SeedDataAsync<ProductSeeder>(StoreContext context, string filePath, ILogger<ProductSeeder> logger)
        {
            if (!File.Exists(filePath))
            {
                logger.LogError($"File not found: {filePath}\n");
                return false;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var products = JsonSerializer.Deserialize<List<Product>>(jsonData);

            foreach (var product in products!)
            {
                var existingProduct = await context.Set<Product>().FirstOrDefaultAsync(t => t.Name == product.Name);
                if (existingProduct == null)
                {
                    context.Set<Product>().Add(product);
                }
            }
            return true;
        }
    }
}
