using AutoMapper;
using Store.Api.DTOs;
using Store.Core.Entites;
using Store.Core.Entites.identity;
using Store.Core.Entites.order_Aggregate;
using System.Net.Sockets;

namespace Store.Api.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductType , O=>O.MapFrom(S=>S.ProductType.Name))
                .ForMember(d => d.ProductBrand , O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
            CreateMap<AddressDto, Store.Core.Entites.order_Aggregate.Address>();
            CreateMap<Core.Entites.order_Aggregate.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                 .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());




        }
    }
}
