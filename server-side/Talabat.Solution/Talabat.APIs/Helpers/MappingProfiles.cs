using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.AuthDTOs;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            #region Mapping Product to ProductDTO
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductBrand, op => op.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, op => op.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.ImageUrl, op => op.MapFrom<ProductImageUrlResolver>())
                .ReverseMap();
            #endregion

            #region Mapping RegisterDTO to ApplicationUser
            CreateMap<RegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.Email, op => op.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, op => op.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.DisplayName, op => op.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.UserName, op => op.MapFrom<UserNameResolver>())
            .ForMember(dest => dest.Address, op => op.MapFrom(src => new Address {
                City = src.City,
                Street = src.Street,
                Building = src.Building,
                Country = src.Country,
                FirstName = src.OrderReciever ?? src.DisplayName,
                ZipCode = src.ZipCode,
                State = src.State
            }));
            #endregion
        }
    }
}
