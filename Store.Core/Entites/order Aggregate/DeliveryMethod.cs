using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entites.order_Aggregate
{
   public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {
            
        }
        public DeliveryMethod(string shortName, string describtion, string deliveryTime, decimal cost)
        {
            ShortName = shortName;
            Describtion = describtion;
            DeliveryTime = deliveryTime;
            Cost = cost;
        }

        public string ShortName { get; set; }
        public string  Describtion { get; set; }
   //     Description
        public string   DeliveryTime { get; set; }
    //    DeliveryTime
        public decimal Cost { get; set; }
       // ShortName

    }
}
