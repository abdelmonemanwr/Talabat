using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Domain.Layer.Entities;

namespace Talabat.APIs.Helpers
{
    public class ProductImageUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration configuration;

        public ProductImageUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return $"{configuration["BaseApiUrl"]}/{source.ImageUrl}";
            }
            return "no image found"; //return null;
        }
    }
}