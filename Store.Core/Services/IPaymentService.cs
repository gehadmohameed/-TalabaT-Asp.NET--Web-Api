using Store.Core.Entites;
using Store.Core.Entites.order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePayementIntent(string BasketId);
        Task<Order> UpdatePaymentIntentToSucceedOrfailed(string PaymentIntentId, bool Flag);
    }
}
