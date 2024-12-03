using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.Entities.Order_Aggregate;

namespace Talabat.Repository.Layer.Data.Seeders
{
    internal class DeliveryMethodSeeder : IDataSeeder
    {
        public async Task<bool> SeedDataAsync<T>(StoreContext context, string filePath, ILogger<T> logger)
        {
            if (!File.Exists(filePath))
            {
                logger.LogError($"File not found: {filePath}\n");
                return false;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(jsonData);

            foreach (var deliveryMethod in deliveryMethods!)
            {
                var existingdeliveryMethod = await context.Set<DeliveryMethod>().FirstOrDefaultAsync(t => t.ShortName == deliveryMethod.ShortName);
                if (existingdeliveryMethod == null)
                {
                    context.Set<DeliveryMethod>().Add(deliveryMethod);
                }
            }
            return true;
        }
    }
}