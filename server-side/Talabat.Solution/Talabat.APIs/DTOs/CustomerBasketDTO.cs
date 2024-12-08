using System.ComponentModel.DataAnnotations;
using Talabat.Domain.Layer.Entities;

namespace Talabat.APIs.DTOs
{
    public class CustomerBasketDTO
    {
        [Required]
        public string Id { get; set; }

        public ICollection<BasketItemDTO> Items { get; set; } = new List<BasketItemDTO>();

        public int? DeliveryMethodId { get; set; }

        public decimal? ShippingPrice { get; set; }

        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }
    }
}
