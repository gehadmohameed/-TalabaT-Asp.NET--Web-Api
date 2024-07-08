using Store.Core.Entites.order_Aggregate;
using System.ComponentModel.DataAnnotations;

namespace Store.Api.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        public int  DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }
    }
}
