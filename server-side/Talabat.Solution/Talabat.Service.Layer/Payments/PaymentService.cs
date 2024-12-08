using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.V2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.Entities.Order_Aggregate;
using Talabat.Domain.Layer.IRepositories;
using Talabat.Domain.Layer.IServices;
using Product = Talabat.Domain.Layer.Entities.Product; // Alias name to avoid conflict with Stripe.Product and Entities.Product

namespace Talabat.Service.Layer.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork unit;
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unit)
        {
            this.unit = unit;
            this.configuration = configuration;
            this.basketRepository = basketRepository;
        }
        public async Task<CustomerBasket?> createOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

            var basket = await basketRepository.GetBasketAsync(basketId);

            if(basket == null) return null;

            //var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                
                if (deliveryMethod == null) 
                    return null;

                basket.ShippingPrice = deliveryMethod.Cost;
            }

            foreach(var product in basket.Items)
            {
                var productItem = await unit.Repository<Product>().GetByIdAsync(product.Id);
                
                if (productItem == null) 
                    return null;

                if (productItem.Price != product.Price)
                {
                    product.Price = productItem.Price; // update price in basket if it was fake price coming from frontend
                }
            }
            
            PaymentIntent paymentIntent;
            var service = new PaymentIntentService();

            // if it doesn't have payment intent id -> create payment intent
            // else if it has payment intent id -> update payment intent

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // create payment intent
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)((basket.Items.Sum(i => i.Price * i.Quantity) + basket.ShippingPrice) * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else  // update payment intent
            {
                var options = new PaymentIntentUpdateOptions() 
                {
                    Amount = (long)((basket.Items.Sum(i => i.Price * i.Quantity) + basket.ShippingPrice) * 100),
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            
            //basket = await basketRepository.UpdateBasketAsync(basket);
            await basketRepository.UpdateBasketAsync(basket);

            return basket;
        }
    }
}
