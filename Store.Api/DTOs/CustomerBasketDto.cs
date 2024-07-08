using Store.Core.Entites;
using System.ComponentModel.DataAnnotations;

namespace Store.Api.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
         public List<BasketItemDto> Items { get; set; }
        public string? PayementIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

    }
}
