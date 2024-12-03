using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities.Order_Aggregate;

namespace Talabat.Domain.Layer.IServices
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address orderShippingAddress);

        Task<IReadOnlyList<Order>> GetUserOrdersAsync(string buyerEmail);

        Task<Order?> GetUserOrderByIdAsync(string buyerEmail, int orderId);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
