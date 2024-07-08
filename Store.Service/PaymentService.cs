using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Store.Core;
using Store.Core.Entites;
using Store.Core.Entites.order_Aggregate;
using Store.Core.Repositories;
using Store.Core.Services;
using Store.Core.Specifications.Order_Spec;
using Stripe;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = Store.Core.Entites.order_Aggregate.Order;
using Product = Store.Core.Entites.Product;

namespace Store.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUniteOfWork _uniteOfWork;

        public PaymentService(IConfiguration  configuration, IBasketRepository basketRepository, IUniteOfWork uniteOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _uniteOfWork = uniteOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePayementIntent(string BasketId)

        {
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null) return null;
            var ShippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _uniteOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }

            if (Basket.Items.Count > 0) {

                foreach (var item in Basket.Items)
                {
                    var Product = await _uniteOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }
            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PayementIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() {"card" }
                };
                paymentIntent = await Service.CreateAsync(Options);
                Basket.PayementIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),

                };
            paymentIntent = await  Service.UpdateAsync(Basket.PayementIntentId, Options);
                Basket.PayementIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrfailed(string PaymentIntentId, bool Flag)
        {

            var Spec = new OrderWithPaymentIntetSpec (PaymentIntentId);
            var Order = await _uniteOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if (Flag)
            {
                Order.Status = OrderStatus.PaymentRecived;
            }
            else
            {
                Order.Status = OrderStatus.PaymentFailed;
            }
            _uniteOfWork.Repository<Order>().Update(Order);
         await   _uniteOfWork.CompleteAsync();
            return Order;

        }
    }
}
