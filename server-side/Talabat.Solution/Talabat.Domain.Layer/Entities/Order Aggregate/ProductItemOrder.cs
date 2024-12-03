using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Domain.Layer.Entities.Order_Aggregate
{
    public class ProductItemOrder
    {
        public ProductItemOrder()
        {

        }

        public ProductItemOrder(int productId, string productName, string imageUrl)
        {
            ProductId = productId;
            ProductName = productName;
            ImageUrl = imageUrl;
        }

        public int ProductId { get; set; }
        
        public string ProductName { get; set; }
        
        public string ImageUrl { get; set; }
    }
}
