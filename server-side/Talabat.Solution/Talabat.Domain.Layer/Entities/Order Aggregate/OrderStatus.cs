using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Domain.Layer.Entities.Order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")] // value to be stored in the database not numbers (0, 1, ...)
        Pending,

        [EnumMember(Value = "Payment Received")] 
        PaymentReceived,

        [EnumMember(Value = "Payment Failed")] 
        PaymentFailed,
    }
}
