using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Api.DTOs;
using Store.Api.Response;
using Store.Core.Entites;
using Store.Core.Services;
using Stripe;

namespace Store.Api.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_f9325123a490c494298b965ba9757046b9beb17b8277a704a38f83398d456e7f";


        //create or update End Point
        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]

        [HttpPost("{ basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
          var CustomerBasket= await  _paymentService.CreateOrUpdatePayementIntent(basketId);
            if (CustomerBasket is null) return BadRequest(new ApiResponse(400, "there is a problem with your basket"));
            var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(CustomerBasket);
            return Ok(MappedBasket);
        }

        [HttpPost("webhook")]

        public async Task<IActionResult> StribeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    _paymentService.UpdatePaymentIntentToSucceedOrfailed(PaymentIntent.Id, false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    _paymentService.UpdatePaymentIntentToSucceedOrfailed(PaymentIntent.Id, true);

                }
                // ... handle other event types
                //else
                //{
                //    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                //}

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
