using Talabat.Domain.Layer.Entities.Order_Aggregate;

namespace Talabat.APIs.DTOs.OrdersDTOs
{
    public class OrderDataDTO
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public string Status { get; set; }

        public Address ShipToAddress { get; set; }

        public string DeliveryMethod { get; set; }

        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDTO> OrderItems { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        public string? PaymentIntentId { get; set; }
    }
}