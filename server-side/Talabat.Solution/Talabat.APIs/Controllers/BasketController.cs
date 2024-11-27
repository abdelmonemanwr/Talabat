﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.IRepositories;

namespace Talabat.APIs.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]   // https://localhost:7294/api/Basket?basketId=Basket1
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string basketId) =>
            Ok(await _basketRepository.GetBasketAsync(basketId) ?? new CustomerBasket(basketId));

        [HttpPost] // https://localhost:7294/api/Basket
        public async Task<ActionResult> UpdateBasket(CustomerBasket basket)
        {
            CustomerBasket? customerBasket = await _basketRepository.UpdateBasketAsync(basket);
            
            var message = customerBasket != null? 
                "The basket was successfully saved to Redis." : 
                "Failed to save the basket to Redis.";
            
            if(customerBasket == null)
                customerBasket = new CustomerBasket(basket.Id);
            
            return Ok(new { Message = message, customerBasket });
        }

        [HttpDelete]  // https://localhost:7294/api/Basket?basketId=Basket1
        public async Task<ActionResult> DeleteBasket(string basketId) =>
            (await _basketRepository.DeleteBasketAsync(basketId))
                ? Ok(new { State = true, Message = "Basket deleted successfully" })
                : NotFound(new { State = false, Message = "Basket wasn't found" });
    }
}