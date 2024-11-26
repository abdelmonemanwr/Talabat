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
    public class ProductTypeSeeder : IDataSeeder
    {
        public async Task<bool> SeedDataAsync(StoreContext context, string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return false;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var types = JsonSerializer.Deserialize<List<ProductType>>(jsonData);

            foreach (var type in types!)
            {
                var existingType = await context.Set<ProductType>().FirstOrDefaultAsync(t => t.Name == type.Name);
                if (existingType == null)
                {
                    context.Set<ProductType>().Add(type);
                }
            }
            return true;
        }
    }
}