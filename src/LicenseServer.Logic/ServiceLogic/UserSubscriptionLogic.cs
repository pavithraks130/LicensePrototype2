using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic
{
    public class UserSubscriptionLogic : BaseLogic
    {
        public List<UserSubscription> GetUserSubscription(string userId)
        {
            List<UserSubscription> subscriptions = new List<UserSubscription>();
            var subscriptionList = Work.UserSubscriptionRepository.GetData(us => us.UserId == userId);
            foreach (var obj in subscriptionList)
            {
                subscriptions.Add(AutoMapper.Mapper.Map<LicenseServer.Core.Model.UserSubscription, DataModel.UserSubscription>(obj));
            }

            return subscriptions;
        }

        public bool CreateUserSubscription(UserSubscription subscription)
        {
            Core.Model.UserSubscription subs = AutoMapper.Mapper.Map<DataModel.UserSubscription, Core.Model.UserSubscription>(subscription);
            var obj = Work.UserSubscriptionRepository.Create(subs);
            Work.UserSubscriptionRepository.Save();
            return obj != null;
        }


    }
}
