using Store.Core;
using Store.Core.Entites;
using Store.Core.Entites.order_Aggregate;
using Store.Core.Repositories;
using Store.Core.Services;
using Store.Core.Specifications.Order_Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository , IUniteOfWork uniteOfWork,IPaymentService paymentService
            )
        {
            _basketRepository = basketRepository;
            _uniteOfWork = uniteOfWork;
            _paymentService = paymentService;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethod = await _uniteOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethod;
        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            var Spec = new OrderSpecification(buyerEmail , OrderId);
            var Order= await _uniteOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            return Order;
        }

        

        public async Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string buyerEmail)
        {
            var Spec = new OrderSpecification(buyerEmail);
            var Orders =  await _uniteOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return Orders;

        }
            
        public async Task<Order?> GreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            var OrderItems = new List<OrderItem> ();
            if (Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _uniteOfWork.Repository<Product>() .GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered,item.Quantity,(int)Product.Price);
                    OrderItems.Add(OrderItem);
                   
                } 
            }
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);
            var DelivaryMethod = await  _uniteOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            var spec = new OrderWithPaymentIntetSpec(Basket.PayementIntentId);
        var ExOrder = await _uniteOfWork.Repository<Order> ().GetEntityWithSpecAsync(spec);
            if(ExOrder is not null)
            {
                _uniteOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePayementIntent(basketId);
            }
            var Order = new Order(buyerEmail, ShippingAddress, DelivaryMethod, OrderItems, SubTotal, Basket.PayementIntentId);
            await _uniteOfWork.Repository<Order>().Add(Order);
        var Result =  await _uniteOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;

        }
    }
}
