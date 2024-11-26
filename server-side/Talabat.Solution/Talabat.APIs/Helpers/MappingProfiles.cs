using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Domain.Layer.Entities;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductBrand, op => op.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, op => op.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.ImageUrl, op => op.MapFrom<ProductImageUrlResolver>())
                .ReverseMap();


        }
    }
}
