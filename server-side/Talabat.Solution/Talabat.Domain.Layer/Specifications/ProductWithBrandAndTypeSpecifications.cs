using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Domain.Layer.Specifications
{
    public enum SortingOptions
    {
        PriceASC,
        PriceDESC,
        NameASC,
        NameDESC
    }

    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecifications() { }

        // Get Product by ID
        public ProductWithBrandAndTypeSpecifications(int id) : base(product => product.Id == id)
        {
            Includes.Add(product => product.ProductType);
            Includes.Add(product => product.ProductBrand);
        }

        // Get All Products
        public ProductWithBrandAndTypeSpecifications(ProductSpecificationParams productSpecificationParams)
                
            /* Short Circuit in C# */
            : base(p => 
                      (!productSpecificationParams.BrandId.HasValue || p.ProductBrandId == productSpecificationParams.BrandId.Value)
                   && (!productSpecificationParams.TypeId.HasValue || p.ProductTypeId == productSpecificationParams.TypeId.Value)
                   && (string.IsNullOrEmpty(productSpecificationParams.Search) || p.Name.Contains(productSpecificationParams.Search))
            )
        {
            Includes.Add(product => product.ProductType);
            Includes.Add(product => product.ProductBrand);

            AddPagination(productSpecificationParams.PageSize * (productSpecificationParams.PageIndex - 1), productSpecificationParams.PageSize);

            AddFilters(productSpecificationParams.Sort);
        }


        private void AddFilters(string? sort)
        {
            //foreach (var s in sort) {} // incase i've set of filters
            if (!string.IsNullOrEmpty(sort) && Enum.TryParse(typeof(SortingOptions), sort, true, out var sortOption))
            {
                switch ((SortingOptions)sortOption)
                {
                    case SortingOptions.PriceASC:
                        AddOrderByAscendingExpression(Entity => Entity.Price);
                        break;
                    case SortingOptions.PriceDESC:
                        AddOrderByDescendingExpression(p => p.Price);
                        break;
                    case SortingOptions.NameASC:
                        AddOrderByAscendingExpression(p => p.Name);
                        break;
                    case SortingOptions.NameDESC:
                        AddOrderByDescendingExpression(p => p.Name);
                        break;
                    default:
                        Console.WriteLine($"Please choose one of the following: [PriceASC, PriceDESC, NameASC, NameDESC]");
                        //AddOrderByDescendingExpression(p => p.Name);
                        break;
                }
            }
        }    
    }
}
