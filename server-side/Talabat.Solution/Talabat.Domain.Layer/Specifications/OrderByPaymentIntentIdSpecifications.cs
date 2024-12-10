using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities.Order_Aggregate;

namespace Talabat.Domain.Layer.Specifications
{
    public class OrderByPaymentIntentIdSpecifications: BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecifications(string paymentIntentId)
            : base(o => o.PaymentIntentId == paymentIntentId) 
        { 

        }
    }
}
