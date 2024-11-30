using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Layer.Data.Seeders
{
    internal interface IDataSeeder
    {
        Task<bool> SeedDataAsync<T>(StoreContext context, string filePath, ILogger<T> logger);
    }
}
