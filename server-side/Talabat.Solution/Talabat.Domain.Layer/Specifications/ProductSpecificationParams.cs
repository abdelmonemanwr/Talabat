using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Domain.Layer.Specifications
{
    public class ProductSpecificationParams
    {
        public int PageIndex { get; set; } = 1;

        private const int MAX_PAGE_SIZE = 10;
        
        public int pageSize = 5;
        
        public int PageSize 
        {  
            get => pageSize; 
            set => pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
        }
        
        public int? BrandId { get; set; }
        
        public int? TypeId { get; set; }

        public string? Sort { get; set; }
        
        public string? Search { get; set; }
    }
}
