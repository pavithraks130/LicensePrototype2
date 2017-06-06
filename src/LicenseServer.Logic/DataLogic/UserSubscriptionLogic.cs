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
            var subscriptionList = Work.UserSubscriptionRepository.GetData(us => us.UserId == userId, null, "SubType");
            subscriptions = subscriptionList.Select(us => AutoMapper.Mapper.Map<DataModel.UserSubscription>(us)).ToList();
            return subscriptions;
        }

        public SubscriptionLicenseMapping CreateUserSubscription(UserSubscription subscription, int teamId)
        {
            Core.Model.UserSubscription subs = AutoMapper.Mapper.Map<DataModel.UserSubscription, Core.Model.UserSubscription>(subscription);
            subs.ActivationDate = DateTime.Now.Date;
            var obj = Work.UserSubscriptionRepository.Create(subs);
            Work.UserSubscriptionRepository.Save();
            UserSubscription sub = null;
            sub = AutoMapper.Mapper.Map<Core.Model.UserSubscription, UserSubscription>(obj);
            if (sub != null)
                return GetSubscriptionWithLicense(sub, teamId);
            return null;
        }

        public SubscriptionList CreateUserSubscriptionList(List<UserSubscription> subsList, string userId)
        {
            UserLogic logic = new UserLogic()
            {
                UserManager = UserManager,
                RoleManager = RoleManager
            };
            User userObj = logic.GetUserById(userId);

            SubscriptionList userSubscriptionList = new SubscriptionList()
            {
                UserId = userId
            };
            int teamId = userObj.OrganizationId;
            foreach (var subObj in subsList)
            {
                var obj = CreateUserSubscription(subObj, teamId);
                userSubscriptionList.Subscriptions.Add(obj);
            }
            return userSubscriptionList;
        }

        public List<LicenseKeyProductMapping> GenerateLicenseKey(UserSubscription subs, int teamId)
        {
            SubscriptionDetailLogic detailLogic = new SubscriptionDetailLogic();
            ProductLogic produLogic = new ProductLogic();
            SubscriptionTypeLogic logic = new SubscriptionTypeLogic();
            Subscription type = logic.GetById(subs.SubscriptionId);
            List<SubscriptionDetails> details = detailLogic.GetSubscriptionDetails(subs.SubscriptionId);
            int qty = subs.Quantity;
            List<LicenseKeyProductMapping> licProductMapping = new List<LicenseKeyProductMapping>();
            List<string> chuksum = new List<string>();

            foreach (var data in details)
            {
                var pro = produLogic.GetProductById(data.ProductId);
                var count = data.Quantity;
                string keyData = pro.ProductCode + "^" + subs.ExpireDate.Date.ToString() + "^" + type.Id + "^" + teamId;
                for (int i = 0; i < (data.Quantity * qty); i++)
                {
                    var key = LicenseKey.LicenseKeyGen.CryptoEngine.Encrypt(keyData, true);
                    string key1 = string.Empty;
                    do
                    {
                        var keygeneration = new LicenseKey.GenerateKey();
                        keygeneration.LicenseTemplate = "xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx";
                        keygeneration.UseBase10 = false;
                        keygeneration.UseBytes = false;
                        keygeneration.CreateKey();
                        key1 = keygeneration.GetLicenseKey();
                    } while (chuksum.Contains(key1));
                    var licKey = key + "-" + key1;
                    licProductMapping.Add(new LicenseKeyProductMapping() { LicenseKey = licKey, ProductId = pro.Id });
                }
            }
            return licProductMapping;
        }

        public List<Subscription> GetExpiringSubscription(string userId)
        {
            List<Subscription> subType = null;
            var subList = Work.UserSubscriptionRepository.GetData(u => u.UserId == userId).ToList();
            var subListObj = subList.Where(s => (s.ExpireDate.Date - DateTime.Now.Date).Days <= 30).ToList();
            subType = subListObj.Select(s => AutoMapper.Mapper.Map<LicenseServer.DataModel.Subscription>(s.Subtype)).ToList();
            return subType;
        }

        public SubscriptionList RenewSubscription(RenewSubscriptionList subList, string userId)
        {
            var user = UserManager.FindByIdAsync(userId).Result;
            List<SubscriptionLicenseMapping> subscriptionListWithLicense = new List<SubscriptionLicenseMapping>();
            var subIdList = subList.SubscriptionList.Select(s => s.Id).ToList();
            var userSubList = Work.UserSubscriptionRepository.GetData(u => u.UserId == userId && subIdList.Contains(u.SubscriptionId)).ToList();
            foreach (var sub in userSubList)
            {
                sub.RenewalDate = subList.RenewalDate;
                sub.ExpireDate = sub.ExpireDate.AddDays(sub.Subtype.ActiveDays);
                Work.UserSubscriptionRepository.Update(sub);
            }
            Work.UserSubscriptionRepository.Save();
            foreach (var sub in userSubList)
            {

                var subTemp = AutoMapper.Mapper.Map<UserSubscription>(sub);
                var subObj = GetSubscriptionWithLicense(subTemp, user.OrganizationId);
                subscriptionListWithLicense.Add(subObj);
            }
            SubscriptionList subListObj = new SubscriptionList();
            subListObj.UserId = userId;
            subListObj.Subscriptions = subscriptionListWithLicense;
            return subListObj;
        }

        public SubscriptionLicenseMapping GetSubscriptionWithLicense(UserSubscription sub, int teamId)
        {
            SubscriptionLicenseMapping purchasedSubscription = new SubscriptionLicenseMapping();
            SubscriptionTypeLogic typeLogic = new SubscriptionTypeLogic();
            SubscriptionDetailLogic detailsLogic = new SubscriptionDetailLogic();

            purchasedSubscription.LicenseKeyProductMapping = GenerateLicenseKey(sub, teamId);
            purchasedSubscription.SubscriptionId = sub.SubscriptionId;
            purchasedSubscription.SubscriptionDate = sub.ActivationDate;
            purchasedSubscription.RenewalDate = sub.RenewalDate;
            purchasedSubscription.OrderdQuantity = sub.Quantity;
            purchasedSubscription.Subscription = typeLogic.GetById(sub.SubscriptionId);
            var details = detailsLogic.GetSubscriptionDetails(purchasedSubscription.SubscriptionId);
            var proList = new List<Product>();
            foreach (var dt in details)
            {
                var pro = dt.Product;
                pro.Quantity = dt.Quantity;
                proList.Add(pro);
            }
            purchasedSubscription.Subscription.Products = proList;
            return purchasedSubscription;
        }
    }
}
