using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Store.Api.DTOs;
using Store.Api.Response;
using Order = Store.Core.Entites.order_Aggregate.Order;
using Store.Core.Services;
using Store.Service;
using System.Security.Claims;
using Store.Core.Entites.identity;
using Store.Core.Entites.order_Aggregate;
using Store.Core;


namespace Store.Api.Controllers
{

    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
       

        public OrdersController(IOrderService orderService, IMapper mapper
            )
        {
            _orderService = orderService;
            _mapper = mapper;
         
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDto, Store.Core.Entites.order_Aggregate.Address>(orderDto.ShippingAddress);
            var Order = await _orderService.GreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);
            if (Order is null) return BadRequest(new ApiResponse(400, "There Is A Problem With Your Order "));
            return Ok(Order);
        }
        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdrsForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrderForSpecificUserAsync(BuyerEmail);
            if (Orders is null) return NotFound(new ApiResponse(404, "There Is No Orders For This User"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>,IReadOnlyList <OrderToReturnDto>>(Orders);
            return Ok(MappedOrders);
        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrderByIdForSpecificUserAsync(BuyerEmail, id);
            if (Orders is null) return NotFound(new ApiResponse(404, $"There Is No Order With id ={id} For This User"));
            var MappedOrder = _mapper.Map<Order, OrderItemDto>(Orders);
            return Ok(MappedOrder);
        }


        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDelivaryMethods()
        {
            var DeliveryMethod = await _orderService.GetDeliveryMethodsAsync();
            return Ok(DeliveryMethod);
        }
    }
}
