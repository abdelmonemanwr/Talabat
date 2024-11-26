using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Repository.Layer.Data.Seeders
{
    public class ProductBrandSeeder : IDataSeeder
    {
        public async Task<bool> SeedDataAsync(StoreContext context, string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return false;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(jsonData);

            foreach (var brand in brands!)
            {
                var existingBrand = await context.Set<ProductBrand>().FirstOrDefaultAsync(t => t.Name == brand.Name);
                if (existingBrand == null)
                {
                    context.Set<ProductBrand>().Add(brand);
                }
            }
            return true;
        }
    }
}
