using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;

namespace License.Logic.ServiceLogic
{
    public class SusbscriptionLogic : BaseLogic
    {
        public List<Subscription> GetAll(string userId)
        {
            List<Subscription> subscriptionList = new List<Subscription>();
            var data = Work.UserSubscriptionRepository.GetData(us => us.UserId == userId);
            var idList = data.Select(s => s.SubscriptionId);
            var subList = Work.SubscriptionRepository.GetData(s => idList.Contains(s.Id));
            foreach (var f in subList)
            {
                subscriptionList.Add(AutoMapper.Mapper.Map<Core.Model.Subscription, Subscription>(f));
            }
            return subscriptionList;
        }

        public bool CreateSubscription(Subscription s)
        {
            var obj = Work.SubscriptionRepository.GetById(s.Id);
            Core.Model.Subscription subs;
            if (obj == null)
            {
                obj = AutoMapper.Mapper.Map<Subscription, Core.Model.Subscription>(s);
                subs = Work.SubscriptionRepository.Create(obj);
                Work.SubscriptionRepository.Save();
            }
            else
            {
                subs = obj;
            }
            return subs.Id > 0;
        }
    }
}
