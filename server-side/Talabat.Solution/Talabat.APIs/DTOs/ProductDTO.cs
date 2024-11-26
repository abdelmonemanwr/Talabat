using Talabat.Domain.Layer.Entities;
#nullable disable

namespace Talabat.APIs.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int ProductBrandId { get; set; }
        public string ProductBrand { get; set; }

        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
    }
}
