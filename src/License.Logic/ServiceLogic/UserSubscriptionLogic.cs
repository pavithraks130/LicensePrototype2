using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;
namespace License.Logic.ServiceLogic
{
    public class UserSubscriptionLogic : BaseLogic
    {
        public List<UserSubscription> GetSubscription(string userId)
        {
            List<UserSubscription> subscriptionList = new List<UserSubscription>();
            LicenseLogic logic = new LicenseLogic();
            var subsList = Work.UserSubscriptionRepository.GetData(us => us.UserId == userId);
            foreach (var obj in subsList)
            {
                var subObj = AutoMapper.Mapper.Map<Core.Model.UserSubscription, UserSubscription>(obj);
                subscriptionList.Add(subObj);
            }
            return subscriptionList;
        }

        public List<UserSubscription> GetSubscriptionByIDList(List<int> idList)
        {
            List<UserSubscription> subscriptionList = new List<UserSubscription>();
            LicenseLogic logic = new LicenseLogic();
            var subsList = Work.UserSubscriptionRepository.GetData(us => idList.Contains(us.Id));
            foreach (var obj in subsList)
            {
                var subObj = AutoMapper.Mapper.Map<Core.Model.UserSubscription, UserSubscription>(obj);
                subscriptionList.Add(subObj);
            }
            return subscriptionList;
        }

        public int CreateSubscription(UserSubscription subs)
        {
            var obj = AutoMapper.Mapper.Map<UserSubscription, License.Core.Model.UserSubscription>(subs);
            obj = Work.UserSubscriptionRepository.Create(obj);
            Work.UserSubscriptionRepository.Save();
            return obj.Id;
        }
    }
}
