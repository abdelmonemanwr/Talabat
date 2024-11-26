using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Domain.Layer.Specifications
{
    public class ProductWithBrandAndTypeSpecifications: BaseSpecification<Product>
    {
        // Get All Products 
        public ProductWithBrandAndTypeSpecifications(): base() {
            Includes.Add(product => product.ProductType);
            Includes.Add(product => product.ProductBrand);
        }

        // Get All Products 
        public ProductWithBrandAndTypeSpecifications(int id)
            : base(product => product.Id == id) {
            Includes.Add(product => product.ProductType);
            Includes.Add(product => product.ProductBrand);
        }
    }
}
