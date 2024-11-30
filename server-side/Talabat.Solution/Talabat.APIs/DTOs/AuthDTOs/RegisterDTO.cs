using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs.AuthDTOs
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }
        
        public string State { get; set; }

        public string ZipCode { get; set; }

        public int Building { get; set; }

        public string Street { get; set; }

        public string OrderReciever { get; set; }
    }
}