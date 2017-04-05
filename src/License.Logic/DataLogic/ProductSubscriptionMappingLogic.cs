using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;

namespace License.Logic.ServiceLogic
{
    public class ProductSubscriptionMappingLogic : BaseLogic
    {
        public bool Create(ProductSubscriptionMapping p)
        {
            var obj = AutoMapper.Mapper.Map<ProductSubscriptionMapping, Core.Model.ProductSubscriptionMapping>(p);
            var obj1 = Work.ProductSubscriptionMapping.Create(obj);
            Work.ProductSubscriptionMapping.Save();
            return obj1 != null;
        }

        public void Create(List<ProductSubscriptionMapping> subscripptionMapList)
        {
            foreach (var obj in subscripptionMapList)
            {
                var mapObj = Work.ProductSubscriptionMapping.GetData(t => t.ProductId == obj.ProductId && t.SubscriptionId == obj.SubscriptionId).FirstOrDefault();
                if (mapObj == null)
                    Create(obj);
            }
        }


    }
}
