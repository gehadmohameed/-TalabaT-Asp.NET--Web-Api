using StackExchange.Redis;
using Store.Core.Entites.order_Aggregate;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.Order_Spec
{
    public class OrderWithPaymentIntetSpec :BaseSpecifications<Entites.order_Aggregate.Order>
        {

        public OrderWithPaymentIntetSpec(string PaymentIntentId):base(O=>O.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
