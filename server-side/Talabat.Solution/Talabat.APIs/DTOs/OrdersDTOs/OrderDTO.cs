#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs.OrdersDTOs
{
    public class OrderDTO
    {
        [Required]
        public string BasketId { get; set; }

        [Required]
        public int DeliveryMethodId { get; set; }

        [Required]
        public AddressDTO ShipToAddress { get; set; }
    }
}
