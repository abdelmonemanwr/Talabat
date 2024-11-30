#nullable disable
namespace Talabat.APIs.DTOs.AuthDTOs
{
    public class AuthResponseDTO
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
