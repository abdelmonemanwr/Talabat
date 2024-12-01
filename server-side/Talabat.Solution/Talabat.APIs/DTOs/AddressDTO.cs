#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class AddressDTO
    {
        public int Id { get; set; }

        [Required]
        public int Building { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string FirstName { get; set; }
    }
}