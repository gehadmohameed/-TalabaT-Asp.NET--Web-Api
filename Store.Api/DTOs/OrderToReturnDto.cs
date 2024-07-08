using Store.Core.Entites.order_Aggregate;

namespace Store.Api.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } 

        public OrderStatus Status { get; set; } 

        public Address ShippingAddress { get; set; }
        public int DeliveryMethodId { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

      //  public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;

        public string? PaymentIntentId { get; set; } 


    }
}
