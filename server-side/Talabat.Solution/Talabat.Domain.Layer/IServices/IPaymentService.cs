using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Domain.Layer.IServices
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> createOrUpdatePaymentIntent(string basketId);
    }
}
