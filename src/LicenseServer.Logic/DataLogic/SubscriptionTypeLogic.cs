using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic
{
    public class SubscriptionTypeLogic : BaseLogic
    {
        public List<SubscriptionType> GetSubscriptionType()
        {
            List<SubscriptionType> subscriptionTypes = new List<SubscriptionType>();
            var listSubscription = Work.SubscriptionRepository.GetData();
            foreach (var obj in listSubscription)
            {
                subscriptionTypes.Add(AutoMapper.Mapper.Map<Core.Model.SubscriptionType, DataModel.SubscriptionType>(obj));
            }

            return subscriptionTypes;
        }

        public bool CreateSubscription(SubscriptionType subType)
        {
            var coreSubType = AutoMapper.Mapper.Map<DataModel.SubscriptionType, Core.Model.SubscriptionType>(subType);
            coreSubType = Work.SubscriptionRepository.Create(coreSubType);
            Work.SubscriptionRepository.Save();
            return coreSubType.Id > 0;
        }

        public SubscriptionType GetById(int id)
        {
            var data = Work.SubscriptionRepository.GetById(id);
            return AutoMapper.Mapper.Map<Core.Model.SubscriptionType, SubscriptionType>(data);
        }
    }
}
