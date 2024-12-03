using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities.Order_Aggregate;

namespace Talabat.Domain.Layer.Specifications
{
    public class OrderWithItemsAndDeliveryMethodOrderByDescendingSpecification: BaseSpecification<Order>
    {
        // get all orders for a specific buyer with order items and delivery method and order by order date descending
        public OrderWithItemsAndDeliveryMethodOrderByDescendingSpecification(string buyerEmail) 
            : base(Criteria: o => o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.OrderItems); 
            // important note about including order items:- 
            // Although the relation is one-to-many, we wanna use eager loading here
            // Bcoz the relation between order and orderItem is composition
            // If the realation was association (aggregation) we wanna use lazy loading

            AddOrderByDescendingExpression(o => o.OrderDate);
        }

        // get a specific order for a specific buyer with order items and delivery method
        public OrderWithItemsAndDeliveryMethodOrderByDescendingSpecification(int orderId, string buyerEmail) :
            base(o => o.Id == orderId && o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.OrderItems);
            Includes.Add(o => o.DeliveryMethod);
        }
    }
}
