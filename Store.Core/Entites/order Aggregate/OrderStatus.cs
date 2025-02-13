﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entites.order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment Received")]
        PaymentRecived,
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed,
    }
}
