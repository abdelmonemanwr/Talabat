 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Domain.Layer.Entities.Order_Aggregate
{
    public class Order: BaseEntity
    {
        public Order() { }

        public Order(string buyerEmail, Address shipToAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> orderItems, decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; }
        
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShipToAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public decimal Subtotal { get; set; } // Total price of all items in the order before delivery, tax, etc.

        public string? PaymentIntentId { get; set; }

        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;
    }
}
