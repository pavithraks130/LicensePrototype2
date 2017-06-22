using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;

namespace LicenseServer.Logic
{
    public class SubscriptionDetailLogic : BaseLogic
    {
        public List<SubscriptionDetails> GetSubscriptionDetails(int subscriptionId)
        {
            List<SubscriptionDetails> details = new List<SubscriptionDetails>();
            var subscriptionList = Work.SubscriptionDetailResitory.GetData(s => s.SubscriptionId == subscriptionId);
            details = subscriptionList.Select(obj => AutoMapper.Mapper.Map<SubscriptionDetails>(obj)).ToList();
            return details;
        }

        public bool CreateSubscriptionDetails(List<SubscriptionDetails> list)
        {
            foreach (var obj in list)
            {
                var subscriptionDetail = AutoMapper.Mapper.Map<SubscriptionDetails, Core.Model.SubscriptionDetail>(obj);
                Work.SubscriptionDetailResitory.Create(subscriptionDetail);
            }
            if (list.Count > 0)
                Work.SubscriptionDetailResitory.Save();
            return true;
        }
    }
}
