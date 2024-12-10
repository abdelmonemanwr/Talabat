using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.Entities.Order_Aggregate;
using Talabat.Domain.Layer.IRepositories;
using Talabat.Domain.Layer.IServices;
using Talabat.Domain.Layer.Specifications;

namespace Talabat.Service.Layer.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unit;
        private readonly IPaymentService paymentService;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unit, IPaymentService paymentService) {
            this.basketRepository = basketRepository;
            this.unit = unit;
            this.paymentService = paymentService;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync() =>
            await unit.Repository<DeliveryMethod>().GetAllAsync();
        
        public async Task<IReadOnlyList<Order>> GetUserOrdersAsync(string buyerEmail)
        {
            var specifications = new OrderWithItemsAndDeliveryMethodOrderByDescendingSpecification(buyerEmail);
            return await unit.Repository<Order>().GetAllAsync(specifications);
        }

        public async Task<Order?> GetUserOrderByIdAsync(string buyerEmail, int orderId)
        {
            var specifications = new OrderWithItemsAndDeliveryMethodOrderByDescendingSpecification(orderId, buyerEmail);
            return await unit.Repository<Order>().GetByIdAsync(specifications);
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address orderShippingAddress)
        {
            CustomerBasket? basket = await basketRepository.GetBasketAsync(basketId);
            
            if (basket == null)
                throw new Exception("Basket not found");

            var orderItems = new List<OrderItem>();

            decimal subTotal = 0;

            foreach (var item in basket!.Items)
            {
                Product? productItem = await unit.Repository<Product>().GetByIdAsync(item.Id);
                
                if(productItem == null)
                    throw new Exception("Product not found");

                var productItemOrder = new ProductItemOrder(productItem.Id, productItem.Name, productItem.ImageUrl);

                var orderItem = new OrderItem(productItem.Price, item.Quantity, productItemOrder);
                orderItems.Add(orderItem);
                // Calculate the subtotal of the order
                subTotal += (item.Quantity * productItem.Price);
            }

            DeliveryMethod? deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            if(deliveryMethod == null)
                throw new Exception("Delivery method not found");

            // check for any existing order with the same payment intent id
            var existingOrder = await unit.Repository<Order>().GetByIdAsync(new OrderByPaymentIntentIdSpecifications(basket.PaymentIntentId));
            if (existingOrder != null)
            {
                unit.Repository<Order>().DeleteAsync(existingOrder);
                await paymentService.createOrUpdatePaymentIntent(basketId);
            }

            var order = new Order(buyerEmail, orderShippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);

            await unit.Repository<Order>().CreateAsync(order);

            int affectedRows = await unit.SaveChangesAsync();
            if (affectedRows <= 0)
                return null;

            return order;
        }
    }
}