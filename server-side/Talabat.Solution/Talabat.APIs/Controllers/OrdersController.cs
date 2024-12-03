using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.OrdersDTOs;
using Talabat.Domain.Layer.Entities.Order_Aggregate;
using Talabat.Domain.Layer.IServices;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, IMapper mapper) 
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods() =>
            Ok(await orderService.GetDeliveryMethodsAsync());

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetAllOrders() => 
            Ok(await orderService.GetUserOrdersAsync(User.FindFirstValue(ClaimTypes.Email)!));
       
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var order = await orderService.GetUserOrderByIdAsync(email, id);
            if (order == null)
                return NotFound("Order not found");
            return Ok(order);
        }

        [HttpPost("create-new-order")]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var orderShippingAddress = mapper.Map<AddressDTO, Address>(orderDTO.ShipToAddress);

            Order? order = await orderService.CreateOrderAsync(email!, orderDTO.BasketId, orderDTO.DeliveryMethodId, orderShippingAddress);

            if (order == null)
                return BadRequest("An error occurred while creating your order please try again later");

            return Ok(order);
        }
    }
}
