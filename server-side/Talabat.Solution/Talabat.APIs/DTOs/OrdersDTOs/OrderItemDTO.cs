namespace Talabat.APIs.DTOs.OrdersDTOs
{
    public class OrderItemDTO
    {
        public int Id { get; set; }         // order item id

        public int ProductId { get; set; }  // product id

        public string ProductName { get; set; }
        
        public string ImageUrl { get; set; }

        public decimal Price { get; set; }
        
        public int Quantity { get; set; }
    }
}
