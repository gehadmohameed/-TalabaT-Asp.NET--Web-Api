using AutoMapper;
using AutoMapper.Execution;
using Store.Api.DTOs;
using Store.Core.Entites;

namespace Store.Api.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, String>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.PictureUrl))
            return $"{_configuration["ApiBaseUrl"]} {source.PictureUrl}";
            return string.Empty;
                
            
        }
    }
}
