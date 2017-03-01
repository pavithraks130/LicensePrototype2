using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic
{
    public class SubscriptionDetailLogic : BaseLogic
    {
        public List<SubscriptionDetails> GetSubscriptionDetails(int subscriptionId)
        {
            List<SubscriptionDetails> details = new List<SubscriptionDetails>();
            var subscriptionList = Work.SubscriptionDetailResitory.GetData(s => s.SubscriptionTypeId == subscriptionId);
            foreach (var obj in subscriptionList)
                details.Add(AutoMapper.Mapper.Map<Core.Model.SubscriptionDetail, DataModel.SubscriptionDetails>(obj));
            return details;
        }

        public bool CreateSubscriptionDetails(List<SubscriptionDetails> list, int subscriptionId)
        {
            foreach(var obj in list)
            {
                var subscriptionDetail = AutoMapper.Mapper.Map<DataModel.SubscriptionDetails, Core.Model.SubscriptionDetail>(obj);
                Work.SubscriptionDetailResitory.Create(subscriptionDetail);
            }
            if (list.Count > 0)
                Work.SubscriptionDetailResitory.Save();
            return true;
        }
    }
}
