using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Domain.Layer.Entities
{
    public class SeedInfo: BaseEntity
    {
        public string SeedName { get; set; }     // Name of the seed (e.g., "ProductTypes")
        public bool IsSeeded { get; set; }       // Tracks whether it's been seeded
        public DateTime LastSeeded { get; set; } // Timestamp of the last seed
    }
}
