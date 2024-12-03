using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.AuthDTOs;
using Talabat.APIs.DTOs.OrdersDTOs;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.Entities.Identity;
using Talabat.Domain.Layer.Entities.Order_Aggregate;

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
                .ForMember(dest => dest.ImageUrl, op => op.MapFrom<ImageUrlResolver<Product, ProductDTO>>())
                .ReverseMap();
            #endregion

            #region Mapping Identity_Address to AddressDTO
            CreateMap<Talabat.Domain.Layer.Entities.Identity.Address, AddressDTO>().ReverseMap();
            #endregion

            #region Mapping Order_Address to AddressDTO
            CreateMap<Talabat.Domain.Layer.Entities.Order_Aggregate.Address, AddressDTO>().ReverseMap();
            #endregion

            #region Mapping RegisterDTO to ApplicationUser
            CreateMap<RegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.Email, op => op.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, op => op.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.DisplayName, op => op.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.UserName, op => op.MapFrom<UserNameResolver>())
            .ForMember(dest => dest.Address, op => op.MapFrom(src => new Talabat.Domain.Layer.Entities.Identity.Address
            {
                City = src.City,
                Street = src.Street,
                Building = src.Building,
                Country = src.Country,
                FirstName = src.OrderReciever ?? src.DisplayName,
                ZipCode = src.ZipCode,
                State = src.State
            }));
            #endregion

            #region Mapping CustomerBasketDTO to CustomerBasket
            CreateMap<CustomerBasketDTO, CustomerBasket>().ReverseMap();
            #endregion

            #region Mapping BasketItemDTO to BasketItem
            CreateMap<BasketItemDTO, BasketItem>().ReverseMap();
            #endregion

            #region Mapping Order to OrderDataDTO
            CreateMap<Order, OrderDataDTO>()
                .ForMember(dest => dest.Status, op => op.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DeliveryMethod, op => op.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.DeliveryMethodCost, op => op.MapFrom(src => src.DeliveryMethod.Cost))
                .ForMember(dest => dest.OrderItems, op => op.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Total, op => op.MapFrom(src => src.GetTotal()))
                .ReverseMap();
            #endregion

            #region Mapping OrderItem to OrderItemDTO
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductId, op => op.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, op => op.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ImageUrl, op => op.MapFrom<ImageUrlResolver<OrderItem, OrderItemDTO>>())
                .ReverseMap();
            #endregion
        }
    }
}
