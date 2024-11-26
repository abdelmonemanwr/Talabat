using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Layer.Data.Seeders
{
    internal interface IDataSeeder
    {
        Task<bool> SeedDataAsync(StoreContext context, string filePath);
    }
}
