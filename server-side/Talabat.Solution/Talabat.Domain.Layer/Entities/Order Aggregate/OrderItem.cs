using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Domain.Layer.Entities.Order_Aggregate
{
    public class OrderItem: BaseEntity
    {
        public OrderItem()
        {
            
        }
 
        public OrderItem(decimal price, int quantity, ProductItemOrder product)
        {
            Price = price;
            Quantity = quantity;
            Product = product;
        }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public ProductItemOrder Product { get; set; } // Builder Design pattern
    }
}
