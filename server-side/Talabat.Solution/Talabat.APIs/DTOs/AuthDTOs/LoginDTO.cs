using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs.AuthDTOs
{
    public class LoginDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
