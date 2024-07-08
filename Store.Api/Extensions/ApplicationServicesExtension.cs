using Microsoft.AspNetCore.Mvc;
using Store.Api.Errors;
using Store.Api.Helpers;
using Store.Core;
using Store.Core.Repositories;
using Store.Core.Services;
using Store.Repository;
using Store.Service;

namespace Store.Api.Extensions
{
    public static class ApplicationServicesExtension
    {

        
        public static IServiceCollection AddApplicationServices( this IServiceCollection Services)
        {
            Services.AddSingleton<IresponseCacheService, ResponseCacheService>();
            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            Services.AddScoped(typeof(IUniteOfWork), typeof(UniteOfWork));
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            // builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            // builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();



            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                .SelectMany(P => P.Value.Errors)
                                                 .Select(E => E.ErrorMessage)
                                                 .ToArray();

                    var ValidationErrorResponse = new ApiValidationResponse()
                    {
                        Errors = errors

                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };

            });

            return Services;
        }
    }
}
