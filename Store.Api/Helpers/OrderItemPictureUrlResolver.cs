using AutoMapper;
using Store.Api.DTOs;
using Store.Core.Entites.order_Aggregate;

namespace Store.Api.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            
            if (string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]} {source.Product.PictureUrl}";
            return string.Empty;
        }
    }
}
