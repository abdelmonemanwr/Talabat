using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace Talabat.Domain.Layer.Entities
{
    public class CustomerBasket
    {
        // This is the user's email address as the basket ID or GUID if the user is not authenticated
        public string Id { get; set; }

        public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();

        // Update constructor parameter name to match property name
        public CustomerBasket(string id) => Id = id;
    }
}
